using Application.Common.DTO.JobOfferReviews;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOfferReviews.Queries.GetJobOfferReviewsOfStudentWithPaging;

public class GetReviewsOfAppliedCVQuery
    : IRequest<IEnumerable<JobOfferReviewDTO>>
{
    public Guid JobOfferId { get; init; }
    public Guid CvId { get; init; }
}

public class GetReviewsOfAppliedCVQueryHandler
    : IRequestHandler<GetReviewsOfAppliedCVQuery, IEnumerable<JobOfferReviewDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsOfAppliedCVQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobOfferReviewDTO>> Handle(
        GetReviewsOfAppliedCVQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.CVJobOffers
            .Where(x => x.CV!.Id == request.CvId && x.JobOffer!.Id == request.JobOfferId)
            .MapToJobOfferReviewDTO()
            .OrderByExpression("created DESC")
            .ToListAsync(cancellationToken);
    }
}