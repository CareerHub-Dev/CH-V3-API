using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Image;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;

public record VerifyCompanyWithContinuedRegistrationCommand : IRequest
{
    public string Token { get; init; } = string.Empty;

    public CreateImage? CompanyLogo { get; init; }
    public CreateImage? CompanyBanner { get; init; }

    public string CompanyName { get; init; } = string.Empty;
    public string CompanyMotto { get; init; } = string.Empty;
    public string CompanyDescription { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class VerifyCompanyWithContinuedRegistrationCommandHandler : IRequestHandler<VerifyCompanyWithContinuedRegistrationCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyCompanyWithContinuedRegistrationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyCompanyWithContinuedRegistrationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.Token);
        }

        Image? imageLogo = null;
        Image? imageBanner = null;

        if(request.CompanyLogo != null)
        {
            imageLogo = new Image { ContentType = request.CompanyLogo.ContentType, Content = request.CompanyLogo.Content };
            await _context.Images.AddAsync(imageLogo, cancellationToken);
        }
        if (request.CompanyBanner != null)
        {
            imageBanner = new Image { ContentType = request.CompanyBanner.ContentType, Content = request.CompanyBanner.Content };
            await _context.Images.AddAsync(imageBanner, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;
        entity.CompanyName = request.CompanyName;
        entity.CompanyMotto = request.CompanyMotto;
        entity.CompanyDescription = request.CompanyDescription;
        entity.CompanyLogoId = imageLogo?.Id;
        entity.CompanyBannerId = imageBanner?.Id;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
