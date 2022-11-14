﻿using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetFollowedShortStudentSubscribersOfStudentForFollowerStudent;

public record GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<FollowedShortStudentDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentSubscriberMustBeVerified { get; init; }
    public Guid? WithoutStudentSubscriberId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedShortStudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedShortStudentDTO>> Handle(
        GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request,
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
                isVerified: request.IsStudentMustBeVerified
            )
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentSubscriberId,
                isVerified: request.IsStudentSubscriberMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Where(x => x.StudentSubscriptions.Any(x => x.SubscriptionTargetId == request.StudentId))
            .Search(request.SearchTerm)
            .MapToFollowedShortStudentDTO(request.FollowerStudentId)
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}