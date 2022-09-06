using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyBanner;

public record UpdateCompanyBannerCommand : IRequest<Guid?>
{
    public Guid CompanyId { get; init; }
    public IFormFile? Banner { get; init; }
}

public class UpdateCompanyBannerCommandHandler : IRequestHandler<UpdateCompanyBannerCommand, Guid?>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyBannerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> Handle(UpdateCompanyBannerCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.BannerId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == company.BannerId);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        if (request.Banner != null)
        {
            var imageBanner = await request.Banner.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(imageBanner);
            company.BannerId = imageBanner.Id;
        }
        else
        {
            company.BannerId = null;
        }

        await _context.SaveChangesAsync();

        return company.BannerId;
    }
}