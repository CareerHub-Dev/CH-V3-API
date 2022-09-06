using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;

public record VerifyCompanyWithContinuedRegistrationCommand : IRequest
{
    public string Token { get; init; } = string.Empty;

    public IFormFile? Logo { get; init; }
    public IFormFile? Banner { get; init; }

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
        var company = await _context.Companies
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.Token);
        }

        company.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        company.VerificationToken = null;
        company.Verified = DateTime.UtcNow;
        company.Name = request.Name;
        company.Motto = request.Motto;
        company.Description = request.Description;

        if (request.Logo != null)
        {
            var imageLogo = await request.Logo.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(imageLogo);
            company.LogoId = imageLogo?.Id;
        }

        if (request.Banner != null)
        {
            var imageBanner = await request.Banner.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(imageBanner);
            company.BannerId = imageBanner?.Id;
        }

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}