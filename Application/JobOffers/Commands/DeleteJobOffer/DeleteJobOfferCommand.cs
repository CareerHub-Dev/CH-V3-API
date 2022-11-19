using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.DeleteJobOffer;

public record DeleteJobOfferCommand(Guid JobOfferId) 
    : IRequest;

public class DeleteJobOfferCommandHandler 
    : IRequestHandler<DeleteJobOfferCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJobOfferCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteJobOfferCommand request, 
        CancellationToken cancellationToken)
    {
        var jobOffer = await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .FirstOrDefaultAsync();

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        _context.JobOffers.Remove(jobOffer);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}