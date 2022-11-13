﻿using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudentForFollowerStudent;

public record GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<StudentSubscriptionOfStudentDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentSubscriptionMustBeVerified { get; init; }
    public Guid? WithoutStudentSubscriptionId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<StudentSubscriptionOfStudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentSubscriptionOfStudentDTO>> Handle(
        GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request,
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
                withoutStudentId: request.WithoutStudentSubscriptionId,
                isVerified: request.IsStudentSubscriptionMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Where(x => x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.StudentId))
            .Search(request.SearchTerm)
            .MapToStudentSubscriptionOfStudentDTO(request.FollowerStudentId)
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}