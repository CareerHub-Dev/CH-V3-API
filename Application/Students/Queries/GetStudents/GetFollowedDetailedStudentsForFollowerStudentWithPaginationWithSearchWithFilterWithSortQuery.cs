using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudents;

public record GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<FollowedDetailedStudentDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedDetailedStudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetailedStudentDTO>> Handle(
        GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request,
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

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm)
            .Select(x => new FollowedDetailedStudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Photo = x.Photo,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                IsFollowed = x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.FollowerStudentId),
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}