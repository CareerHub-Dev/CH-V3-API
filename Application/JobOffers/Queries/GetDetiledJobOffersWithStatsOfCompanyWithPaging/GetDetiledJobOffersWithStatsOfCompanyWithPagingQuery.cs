using Application.Common.DTO.JobOffers;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.JobOffers.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetDetiledJobOffersWithStatsOfCompanyWithPaging;

public record GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery
    : IRequest<PaginatedList<DetiledJobOfferWithStatsDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsJobOfferMustBeActive { get; init; }
    public JobType? MustHaveJobType { get; init; }
    public WorkFormat? MustHaveWorkFormat { get; init; }
    public ExperienceLevel? MustHaveExperienceLevel { get; init; }
    public Guid? MustHaveJobPositionId { get; init; }
    public List<Guid>? MustHaveTagIds { get; init; } = new List<Guid>();

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetDetiledJobOffersWithStatsOfCompanyWithPagingQueryHandler
    : IRequestHandler<GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery, PaginatedList<DetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetDetiledJobOffersWithStatsOfCompanyWithPagingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DetiledJobOfferWithStatsDTO>> Handle(
        GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.JobOffers
            .AsNoTracking()
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                jobType: request.MustHaveJobType,
                workFormat: request.MustHaveWorkFormat,
                experienceLevel: request.MustHaveExperienceLevel,
                jobPositionId: request.MustHaveJobPositionId,
                tagIds: request.MustHaveTagIds
            )
            .Where(x => x.CompanyId == request.CompanyId)
            .Search(request.SearchTerm)
            .MapToDetiledJobOfferWithStatsDTO(
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified,
                isStudentOfAppliedCVMustBeVerified: request.StatsFilter.IsStudentOfAppliedCVMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}