using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.DTO.StudentGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Application.Common.DTO.Students;

namespace Application.Students.Queries.GetStudents;

public record GetStudentsWithPaginationWithSearthWithFilterWithSortQuery : IRequest<PaginatedList<StudentDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentsWithPaginationWithSearthWithFilterWithSortQueryHandler : IRequestHandler<GetStudentsWithPaginationWithSearthWithFilterWithSortQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsWithPaginationWithSearthWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(GetStudentsWithPaginationWithSearthWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .Search(request.SearchTerm)
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
                PasswordReset = x.PasswordReset
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}