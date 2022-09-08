using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.Models.StudentGroup;
using Application.Students.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudents;

public record GetStudentsWithPaginationWithSearthWithFilterQuery : IRequest<PaginatedList<StudentDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
}

public class GetStudentsWithPaginationWithSearthWithFilterQueryHandler : IRequestHandler<GetStudentsWithPaginationWithSearthWithFilterQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsWithPaginationWithSearthWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(GetStudentsWithPaginationWithSearthWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .AsNoTracking()
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
                StudentGroup = new StudentGroupBriefDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}