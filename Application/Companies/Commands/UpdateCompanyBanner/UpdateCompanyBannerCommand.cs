using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyBanner;

public record UpdateCompanyBannerCommand : IRequest<string?>
{
    public Guid CompanyId { get; init; }
    public IFormFile? Banner { get; init; }
}

public class UpdateCompanyBannerCommandHandler : IRequestHandler<UpdateCompanyBannerCommand, string?>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public UpdateCompanyBannerCommandHandler(IApplicationDbContext context, IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<string?> Handle(UpdateCompanyBannerCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (!string.IsNullOrWhiteSpace(company.Banner))
        {
            _imagesService.DeleteImageIfExists(Path.GetFileName(company.Banner));
            company.Banner = null;
        }

        if (request.Banner != null)
        {
            company.Banner = await _imagesService.SaveImageAsync(request.Banner);
        }

        await _context.SaveChangesAsync();

        return company.Banner;
    }
}