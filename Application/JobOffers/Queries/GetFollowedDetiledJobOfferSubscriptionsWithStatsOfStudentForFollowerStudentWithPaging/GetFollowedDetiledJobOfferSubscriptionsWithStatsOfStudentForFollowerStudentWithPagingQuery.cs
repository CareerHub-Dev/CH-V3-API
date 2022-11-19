using Application.Common.DTO.JobOffers;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.JobOffers.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPaging;

public record GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery
    : IRequest<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }

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

public class GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryQueryHandler
    : IRequestHandler<GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery, PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>> Handle(
        GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        if (!await _context.Students
            .Filter(isVerified: request.IsStudentOwnerMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                jobType: request.MustHaveJobType,
                workFormat: request.MustHaveWorkFormat,
                experienceLevel: request.MustHaveExperienceLevel,
                jobPositionId: request.MustHaveJobPositionId,
                tagIds: request.MustHaveTagIds,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentOwnerId))
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