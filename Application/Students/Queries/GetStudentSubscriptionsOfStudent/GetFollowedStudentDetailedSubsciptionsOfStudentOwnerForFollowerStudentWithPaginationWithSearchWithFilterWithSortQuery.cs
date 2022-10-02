﻿using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudent;

public record GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<FollowedDetailedStudentDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }
    public ActivationStatus? FollowerStudentMustHaveActivationStatus { get; init; }

    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }
    public ActivationStatus? StudentOwnerMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedDetailedStudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetailedStudentDTO>> Handle(GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsFollowerStudentMustBeVerified,
                activationStatus: request.FollowerStudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentOwnerMustBeVerified,
                activationStatus: request.StudentOwnerMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .Where(x => x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.StudentOwnerId))
            .Search(request.SearchTerm)
            .Select(x => new FollowedDetailedStudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhotoId = x.PhotoId,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                IsFollowed = x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.FollowerStudentId),
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}