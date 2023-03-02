using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetAmountAppliedCVsOfJobOffer;

public record GetAmountAppliedCVsOfJobOfferQuery
    : IRequest<int>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public bool? IsStudentOfAppliedCVMustBeVerified { get; init; }
}

public class GetAmountAppliedCVsOfJobOfferQueryHandler
    : IRequestHandler<GetAmountAppliedCVsOfJobOfferQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountAppliedCVsOfJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountAppliedCVsOfJobOfferQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .SelectMany(x => x.CVJobOffers)
            .Filter(isStudentOfCVVerified: request.IsStudentOfAppliedCVMustBeVerified)
            .CountAsync();
    }
}