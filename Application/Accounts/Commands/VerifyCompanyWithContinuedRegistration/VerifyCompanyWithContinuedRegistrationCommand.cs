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

    public CreateImage? Logo { get; init; }
    public CreateImage? Banner { get; init; }

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

        var imageLogo = request.Logo?.ToImageWithGeneratedId;
        var imageBanner = request.Banner?.ToImageWithGeneratedId;

        if (imageLogo != null) await _context.Images.AddAsync(imageLogo, cancellationToken);
        if (imageBanner != null) await _context.Images.AddAsync(imageBanner, cancellationToken);

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