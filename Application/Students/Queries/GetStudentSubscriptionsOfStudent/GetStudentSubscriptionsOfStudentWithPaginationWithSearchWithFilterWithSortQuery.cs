﻿using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudent;

public record GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<StudentDTO>>
{
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

public class GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(
        GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery request,
        CancellationToken cancellationToken)
    {
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
            .Where(x => x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.StudentId))
            .Filter(
                withoutStudentId: request.WithoutStudentSubscriptionId,
                isVerified: request.IsStudentSubscriptionMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm)
            .MapToStudentDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}