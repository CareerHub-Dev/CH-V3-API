using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.DTO.StudentGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;
using Application.Common.DTO.Students;

namespace Application.Students.Queries.GetStudents;

public record GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<FollowedStudentDetailedDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }
    public ActivationStatus? FollowerStudentMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedStudentDetailedDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedStudentDetailedDTO>> Handle(GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
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

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .Search(request.SearchTerm)
            .Select(x => new FollowedStudentDetailedDTO
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