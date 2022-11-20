using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPaging;

public record GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery
    : IRequest<PaginatedList<FollowedShortCompanyWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryHandler
    : IRequestHandler<GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery, PaginatedList<FollowedShortCompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedShortCompanyWithStatsDTO>> Handle(
        GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Companies
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm)
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentId))
            .MapToFollowedShortCompanyWithStatsDTO(
                request.FollowerStudentId,
                isJobOfferMustBeActive: request.StatsFilter.IsJobOfferMustBeActive,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}