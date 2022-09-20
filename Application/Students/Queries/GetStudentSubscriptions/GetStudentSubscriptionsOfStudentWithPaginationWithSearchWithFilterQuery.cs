using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.Models.StudentGroup;
using Application.Students.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscriptions;

public record GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
}

public class GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .AsNoTracking()
            .Where(x => x.StudentsSubscribed.Any(x => x.SubscriptionOwnerId == request.StudentId))
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            .Select(x => new StudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhotoId = x.PhotoId,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}