using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Image;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyLogo;

public record UpdateCompanyLogoCommand : IRequest<Guid?>
{
    public Guid CompanyId { get; init; }
    public CreateImage? Logo { get; init; }
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
        var entity = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (entity.CompanyBannerId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.CompanyLogoId, cancellationToken);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        var newImage = request.Logo?.ToImageWithGeneratedId;

        if (newImage != null)
        {
            await _context.Images.AddAsync(newImage, cancellationToken);
        }

        entity.CompanyLogoId = newImage?.Id;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.CompanyLogoId;
    }
}