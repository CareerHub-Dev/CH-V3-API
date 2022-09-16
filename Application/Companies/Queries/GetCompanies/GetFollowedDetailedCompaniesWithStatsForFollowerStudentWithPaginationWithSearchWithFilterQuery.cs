using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanies;

public record GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<FollowedDetailedCompanyWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();
}

public class GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<FollowedDetailedCompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetailedCompanyWithStatsDTO>> Handle(GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentMustBeVerified)
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
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new FollowedDetailedCompanyWithStatsDTO
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
                IsFollowed = x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId),
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}