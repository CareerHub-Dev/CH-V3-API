using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Image;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyBanner;

public record UpdateCompanyBannerCommand : IRequest<Guid?>
{
    public Guid CompanyId { get; init; }
    public CreateImage? Banner { get; init; }
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

        var newImage = request.Banner?.ToImageWithGeneratedId;

        if (newImage != null)
        {
            await _context.Images.AddAsync(newImage);
        }

        company.BannerId = newImage?.Id;

        await _context.SaveChangesAsync();

        return company.BannerId;
    }
}