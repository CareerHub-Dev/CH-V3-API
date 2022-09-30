using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanies;

public record GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<CompanyWithStatsDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }
    public ActivationStatus? CompanyMustHaveActivationStatus { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<CompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithStatsDTO>> Handle(
        GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQuery request, 
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified,
                activationStatus: request.CompanyMustHaveActivationStatus
            )
            .Search(request.SearchTerm)
            .Select(x => new CompanyWithStatsDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                AmountJobOffers = x.JobOffers.Count(x =>
                    !request.StatsFilter.IsJobOfferMustBeActive.HasValue || (request.StatsFilter.IsJobOfferMustBeActive.Value ?
                            x.EndDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow :
                            x.StartDate > DateTime.UtcNow
                        )
                ),
                AmountSubscribers = x.SubscribedStudents.Count(x =>
                    (!request.StatsFilter.IsSubscriberMustBeVerified.HasValue || (request.StatsFilter.IsSubscriberMustBeVerified.Value ?
                            x.Verified != null || x.PasswordReset != null :
                            x.Verified == null && x.PasswordReset == null
                       ))
                    &&
                    (!request.StatsFilter.ActivationStatusOfSubscriber.HasValue || (x.ActivationStatus == request.StatsFilter.ActivationStatusOfSubscriber))
                ),
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                ActivationStatus = x.ActivationStatus,
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
