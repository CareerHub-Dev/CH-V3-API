using Application.Common.DTO.JobOffers;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetJobOffer;

public record GetJobOfferQuery
    : IRequest<JobOfferDTO>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
}

public class GetJobOfferQueryHandler
    : IRequestHandler<GetJobOfferQuery, JobOfferDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOfferDTO> Handle(
        GetJobOfferQuery request,
        CancellationToken cancellationToken)
    {
        var jobOffer = await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .MapToJobOfferDTO()
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return jobOffer;
    }
}