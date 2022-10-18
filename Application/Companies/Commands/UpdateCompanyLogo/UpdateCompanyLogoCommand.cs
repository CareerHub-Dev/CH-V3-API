using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyLogo;

public record UpdateCompanyLogoCommand : IRequest<string?>
{
    public Guid CompanyId { get; init; }
    public IFormFile? Logo { get; init; }
}

public class UpdateCompanyLogoCommandHandler : IRequestHandler<UpdateCompanyLogoCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public UpdateCompanyLogoCommandHandler(IApplicationDbContext context, IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (!string.IsNullOrWhiteSpace(company.Logo))
        {
            _imagesService.DeleteImageIfExists(Path.GetFileName(company.Logo));
            company.Logo = null;
        }

        if (request.Logo != null)
        {
            company.Logo = await _imagesService.SaveImageAsync(request.Logo);
        }

        await _context.SaveChangesAsync();

        return company.Logo;
    }
}