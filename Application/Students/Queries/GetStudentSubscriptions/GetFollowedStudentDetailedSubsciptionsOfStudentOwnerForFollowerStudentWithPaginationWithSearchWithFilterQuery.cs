using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Students.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscriptions;

public record GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<FollowedStudentDetailedDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentVerified { get; init; }

    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerVerified { get; init; }

    //FollowedStudentDetaileds filters
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
}

public class GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<FollowedStudentDetailedDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedStudentDetailedDTO>> Handle(GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException("FollowerStudent", request.FollowerStudentId);
        }

        if (!await _context.Students
            .Filter(isVerified: request.IsStudentOwnerVerified)
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException("StudentOwner", request.StudentOwnerId);
        }

        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Where(x => x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.StudentOwnerId))
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