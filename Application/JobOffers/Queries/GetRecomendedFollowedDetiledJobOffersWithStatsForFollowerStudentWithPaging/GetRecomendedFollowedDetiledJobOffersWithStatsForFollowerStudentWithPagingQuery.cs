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
using System.Threading.Tasks;

namespace Application.JobOffers.Queries.GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;

public record GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery
    : IRequest<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

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

public class GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryQueryHandler
    : IRequestHandler<GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery, PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>> Handle(
        GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsFollowerStudentMustBeVerified
            )
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        var tags = await _context.JobOffers
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId))
            .SelectMany(x => x.Tags).Select(x => x.Id).Distinct().ToListAsync();

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
            .Where(x => x.Tags.Select(x => x.Id).Count(y => tags.Contains(y))/x.Tags.Count() > 0.75)
            .Search(request.SearchTerm)
            .MapToFollowedDetiledJobOfferWithStatsDTO(
                request.FollowerStudentId,
                isStudentOfAppliedCVMustBeVerified: request.StatsFilter.IsStudentOfAppliedCVMustBeVerified,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}