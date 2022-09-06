using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyLogo;

public record UpdateCompanyLogoCommand : IRequest<Guid?>
{
    public Guid CompanyId { get; init; }
    public IFormFile? Logo { get; init; }
}

public class UpdateCompanyLogoCommandHandler : IRequestHandler<UpdateCompanyLogoCommand, Guid?>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyLogoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.BannerId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == company.LogoId);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        if (request.Logo != null)
        {
            var imageLogo = await request.Logo.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(imageLogo);
            company.LogoId = imageLogo.Id;
        }
        else
        {
            company.LogoId = null;
        }

        await _context.SaveChangesAsync();

        return company.LogoId;
    }
}