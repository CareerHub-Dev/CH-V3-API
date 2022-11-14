using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetBriefCompaniesWithStatsWithPaginig;

public record GetBriefCompaniesWithStatsWithPagingQuery
    : IRequest<PaginatedList<BriefCompanyWithStatsDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCompaniesWithStatsWithPagingQueryHandler
    : IRequestHandler<GetBriefCompaniesWithStatsWithPagingQuery, PaginatedList<BriefCompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCompaniesWithStatsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCompanyWithStatsDTO>> Handle(
        GetBriefCompaniesWithStatsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm)
            .MapToBriefCompanyWithStatsDTO(
                isJobOfferMustBeActive: request.StatsFilter.IsJobOfferMustBeActive,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
