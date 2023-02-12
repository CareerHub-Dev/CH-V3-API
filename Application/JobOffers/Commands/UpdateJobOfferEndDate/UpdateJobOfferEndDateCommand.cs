using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.UpdateJobOfferEndDate;

public record UpdateJobOfferEndDateCommand
    : IRequest
{
    public Guid JobOfferId { get; init; }

    public DateTime EndDate { get; init; }
}

public class UpdateJobOfferEndDateCommandHandler
    : IRequestHandler<UpdateJobOfferEndDateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobOfferEndDateCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateJobOfferEndDateCommand request,
        CancellationToken cancellationToken)
    {
        var jobOffer = await _context.JobOffers
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        jobOffer.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}