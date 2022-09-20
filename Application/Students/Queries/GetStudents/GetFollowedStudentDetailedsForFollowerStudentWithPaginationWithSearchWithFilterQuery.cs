using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.DTO.StudentGroups;
using Application.Students.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudents;

public record GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<FollowedStudentDetailedDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentVerified { get; init; }

    //FollowedStudentDetaileds filters
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
}

public class GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<FollowedStudentDetailedDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedStudentDetailedDTO>> Handle(GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException("FollowerStudent", request.FollowerStudentId);
        }

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
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
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}