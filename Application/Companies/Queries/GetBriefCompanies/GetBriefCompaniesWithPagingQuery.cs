using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using MediatR;

namespace Application.Companies.Queries.GetBriefCompanies;

public record GetBriefCompaniesWithPagingQuery
    : IRequest<PaginatedList<BriefCompanyDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCompaniesWithPagingQueryHandler
    : IRequestHandler<GetBriefCompaniesWithPagingQuery, PaginatedList<BriefCompanyDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCompaniesWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCompanyDTO>> Handle(
        GetBriefCompaniesWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .Filter(
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm)
            .MapToBriefCompanyDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
