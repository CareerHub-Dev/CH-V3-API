using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanies;

public record GetCompaniesWithStatsWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<CompanyWithStatsDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();
}

public class GetCompaniesWithStatsWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetCompaniesWithStatsWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompaniesWithStatsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithStatsDTO>> Handle(
        GetCompaniesWithStatsWithPaginationWithSearchWithFilterQuery request, 
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new CompanyWithStatsDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                AmountJobOffers = x.JobOffers.Filter(request.StatsFilter.IsJobOfferMustBeActive, null).Count(),
                AmountSubscribers = x.SubscribedStudents.Filter(null, request.StatsFilter.IsSubscriberMustBeVerified, null).Count(),
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
