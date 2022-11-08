using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscribersOfStudent;

public record GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentOwnerMustBeVerified
            )
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.Students
            .AsNoTracking()
            .Where(x => x.StudentSubscriptions.Any(x => x.SubscriptionTargetId == request.StudentOwnerId))
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm)
            .Select(x => new StudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Photo = x.Photo,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}