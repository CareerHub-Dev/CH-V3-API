using Application.Common.DTO.JobOfferReviews;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOfferReviews.Queries.GetJobOfferReviewsOfStudentWithPaging;

public class GetJobOfferReviewsOfStudentWithPagingQuery
    : IRequest<PaginatedList<JobOfferReviewDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetJobOfferReviewsOfStudentWithPagingQueryHandler
    : IRequestHandler<GetJobOfferReviewsOfStudentWithPagingQuery, PaginatedList<JobOfferReviewDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferReviewsOfStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<JobOfferReviewDTO>> Handle(
        GetJobOfferReviewsOfStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.CVJobOffers
            .Where(x => x.CV!.StudentId == request.StudentId)
            .MapToJobOfferReviewDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}