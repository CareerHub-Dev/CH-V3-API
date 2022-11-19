﻿using Application.Common.DTO.JobOffers;
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

namespace Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;

public record GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
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
    public JobType? MustHaveJobType { get; set; }
    public WorkFormat? MustHaveWorkFormat { get; set; }
    public ExperienceLevel? MustHaveExperienceLevel { get; set; }
    public Guid? MustHaveJobPositionId { get; set; }
    public List<Guid>? MustHaveTagIds { get; set; } = new List<Guid>();
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>> Handle(
        GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request,
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

        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentOwnerMustBeVerified
            )
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.JobOffers
            .AsNoTracking()
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
            .Select(x => new FollowedDetiledJobOfferWithStatsDTO
            {
                Id = x.Id,
                Title = x.Title,
                Image = x.Image,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                JobType = x.JobType,
                WorkFormat = x.WorkFormat,
                ExperienceLevel = x.ExperienceLevel,
                JobPosition = new BriefJobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
                Company = new BriefCompanyOfJobOfferDTO { Id = x.Company!.Id, Name = x.Company.Name },
                Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
                IsFollowed = x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId),
                AmountSubscribers = x.SubscribedStudents.Count(x =>
                    (!request.StatsFilter.IsSubscriberMustBeVerified.HasValue || (request.StatsFilter.IsSubscriberMustBeVerified.Value ?
                            x.Verified != null || x.PasswordReset != null :
                            x.Verified == null && x.PasswordReset == null
                       ))
                ),
                AmountAppliedCVs = x.AppliedCVs.Count(x =>
                    (!request.StatsFilter.IsStudentOfAppliedCVMustBeVerified.HasValue || (request.StatsFilter.IsStudentOfAppliedCVMustBeVerified.Value ?
                            x.Student!.Verified != null || x.Student.PasswordReset != null :
                            x.Student!.Verified == null && x.Student.PasswordReset == null
                       ))
                )
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}