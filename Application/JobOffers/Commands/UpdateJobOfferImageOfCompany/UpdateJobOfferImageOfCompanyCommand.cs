using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;

public record UpdateJobOfferImageOfCompanyCommand : IRequest<Guid?>
{
    public Guid JobofferId { get; init; }
    public IFormFile? Image { get; init; }

    public Guid CompanyId { get; init; }
}

public class UpdateJobOfferImageOfCompanyCommandHandler : IRequestHandler<UpdateJobOfferImageOfCompanyCommand, Guid?>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobOfferImageOfCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> Handle(UpdateJobOfferImageOfCompanyCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var jobOffer = await _context.JobOffers
            .FirstOrDefaultAsync(x => x.Id == request.JobofferId && x.CompanyId == request.CompanyId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobofferId);
        }

        if (jobOffer.ImageId != null)
        {
            var image = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == jobOffer.ImageId);

            if (image != null)
            {
                _context.Images.Remove(image);
            }
        }

        if (request.Image != null)
        {
            var image = await request.Image.ToImageWithGeneratedIdAsync();
            await _context.Images.AddAsync(image);
            jobOffer.ImageId = image.Id;
        }
        else
        {
            jobOffer.ImageId = null;
        }

        await _context.SaveChangesAsync();

        return jobOffer.ImageId;
    }
}