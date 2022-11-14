using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetFollowedShortCompaniesWithStatsForFollowerStudentWithPaginig;

public record GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQuery
    : IRequest<PaginatedList<FollowedShortCompanyWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQueryHandler
    : IRequestHandler<GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQuery, PaginatedList<FollowedShortCompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedShortCompanyWithStatsDTO>> Handle(
        GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsFollowerStudentMustBeVerified
            )
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm)
            .MapToFollowedShortCompanyWithStatsDTO(
                request.FollowerStudentId,
                isJobOfferMustBeActive: request.StatsFilter.IsJobOfferMustBeActive,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}