using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.UpdateJobOfferImage;

public record UpdateJobOfferImageCommand : IRequest<Guid?>
{
    public Guid JobofferId { get; init; }
    public IFormFile? Image { get; init; }
}

public class UpdateJobOfferImageCommandHandler : IRequestHandler<UpdateJobOfferImageCommand, Guid?>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobOfferImageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> Handle(UpdateJobOfferImageCommand request, CancellationToken cancellationToken)
    {
        var jobOffer = await _context.JobOffers
            .FirstOrDefaultAsync(x => x.Id == request.JobofferId);

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