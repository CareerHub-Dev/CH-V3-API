using Application.Common.DTO.CompanyLinks;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

    public List<CompanyLinkDTO> Links { get; init; } = new List<CompanyLinkDTO>();

    public string Password { get; init; } = string.Empty;
}

public class VerifyCompanyWithContinuedRegistrationCommandHandler : IRequestHandler<VerifyCompanyWithContinuedRegistrationCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IImagesService _imagesService;

    public VerifyCompanyWithContinuedRegistrationCommandHandler(
        IApplicationDbContext context, 
        IPasswordHasher<Account> passwordHasher,
        IImagesService imagesService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _imagesService = imagesService;
    }

    public async Task<Unit> Handle(VerifyCompanyWithContinuedRegistrationCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.Token);
        }

        company.PasswordHash = _passwordHasher.HashPassword(company, request.Password);
        company.VerificationToken = null;
        company.Verified = DateTime.UtcNow;

        company.Name = request.Name;
        company.Motto = request.Motto;
        company.Description = request.Description;
        company.Logo = request.Logo != null ? await _imagesService.SaveImageAsync(request.Logo) : null;
        company.Banner = request.Banner != null ? await _imagesService.SaveImageAsync(request.Banner) : null;

        company.Links = request.Links
            .Select(x => new CompanyLink { Title = x.Title, Uri = x.Uri })
            .ToList();

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}