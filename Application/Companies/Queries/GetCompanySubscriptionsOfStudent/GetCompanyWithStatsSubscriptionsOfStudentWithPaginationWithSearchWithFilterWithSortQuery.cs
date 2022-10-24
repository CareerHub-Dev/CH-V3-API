using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent;

public record GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<CompanyWithStatsDTO>>
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }
    public ActivationStatus? StudentOwnerMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }
    public ActivationStatus? CompanyMustHaveActivationStatus { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<CompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithStatsDTO>> Handle(
        GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentOwnerMustBeVerified,
                activationStatus: request.StudentOwnerMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified,
                activationStatus: request.CompanyMustHaveActivationStatus
            )
            .Search(request.SearchTerm)
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentOwnerId))
            .Select(x => new CompanyWithStatsDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Logo = x.Logo,
                Banner = x.Banner,
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
                    (!request.StatsFilter.SubscriberMustHaveActivationStatus.HasValue || (x.ActivationStatus == request.StatsFilter.SubscriberMustHaveActivationStatus))
                ),
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                ActivationStatus = x.ActivationStatus
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
