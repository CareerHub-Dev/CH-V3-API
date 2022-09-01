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

    public string Name { get; init; } = string.Empty;
    public string Motto { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
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
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.Token);
        }

        var imageLogo = request.Logo?.ToImageWithGeneratedId;
        var imageBanner = request.Banner?.ToImageWithGeneratedId;

        if (imageLogo != null) await _context.Images.AddAsync(imageLogo);
        if (imageBanner != null) await _context.Images.AddAsync(imageBanner);

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;
        entity.Name = request.Name;
        entity.Motto = request.Motto;
        entity.Description = request.Description;
        entity.LogoId = imageLogo?.Id;
        entity.BannerId = imageBanner?.Id;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}