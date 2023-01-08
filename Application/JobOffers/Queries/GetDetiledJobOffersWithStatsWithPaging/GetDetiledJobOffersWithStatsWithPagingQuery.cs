using Application.Common.DTO.JobOffers;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.JobOffers.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.JobOffers.Queries.GetDetiledJobOffersWithStatsWithPaging;

public record GetDetiledJobOffersWithStatsWithPagingQuery
    : IRequest<PaginatedList<DetiledJobOfferWithStatsDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsJobOfferMustBeActive { get; init; }
    public JobType? MustHaveJobType { get; init; }
    public WorkFormat? MustHaveWorkFormat { get; init; }
    public ExperienceLevel? MustHaveExperienceLevel { get; init; }
    public Guid? MustHaveJobPositionId { get; init; }
    public List<Guid>? MustHaveTagIds { get; init; } = new List<Guid>();
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetDetiledJobOffersWithStatsWithPagingQueryHandler
    : IRequestHandler<GetDetiledJobOffersWithStatsWithPagingQuery, PaginatedList<DetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetDetiledJobOffersWithStatsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DetiledJobOfferWithStatsDTO>> Handle(
        GetDetiledJobOffersWithStatsWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        return await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified,
                jobType: request.MustHaveJobType,
                workFormat: request.MustHaveWorkFormat,
                experienceLevel: request.MustHaveExperienceLevel,
                jobPositionId: request.MustHaveJobPositionId,
                tagIds: request.MustHaveTagIds
            )
            .Search(request.SearchTerm)
            .MapToDetiledJobOfferWithStatsDTO(
                isStudentOfAppliedCVMustBeVerified: request.StatsFilter.IsStudentOfAppliedCVMustBeVerified,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}