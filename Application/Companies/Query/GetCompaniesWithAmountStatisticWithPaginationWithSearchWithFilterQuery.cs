using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Query.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQuery 
    : IRequest<PaginatedList<CompanyWithAmountStatisticDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public bool? IsJobOfferActive { get; init; }
    public bool? IsSubscriberVerified { get; init; }
}

public class GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyWithAmountStatisticDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithAmountStatisticDTO>> Handle(GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId, 
                isVerified: request.IsVerified
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new CompanyWithAmountStatisticDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                AmountStatistic = new AmountStatistic
                {
                    AmountJobOffers = x.JobOffers.Filter(request.IsJobOfferActive, null).Count(),
                    AmountSubscribers = x.SubscribedStudents.Filter(null, request.IsSubscriberVerified, null).Count()
                },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
