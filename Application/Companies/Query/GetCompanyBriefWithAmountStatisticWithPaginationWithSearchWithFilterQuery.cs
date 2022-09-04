using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Query.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQuery : IRequest<PaginatedList<CompanyBriefWithAmountStatisticDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public bool? IsJobOfferActive { get; init; }
    public bool? IsSubscriberVerified { get; init; }
}

public class GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQueryHandler : IRequestHandler<GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyBriefWithAmountStatisticDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyBriefWithAmountStatisticDTO>> Handle(GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(request.WithoutCompanyId, request.IsVerified)
            .Search(request.SearchTerm ?? "")
            .Select(x => new CompanyBriefWithAmountStatisticDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                AmountStatistic = new AmountStatistic
                {
                    AmountJobOffers = x.JobOffers.Filter(request.IsSubscriberVerified).Count(),
                    AmountSubscribers = x.SubscribedStudents.Filter(null, request.IsSubscriberVerified).Count()
                }
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
