using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudents;

public record GetStudentsWithPagingQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentsWithPagingQueryHandler
    : IRequestHandler<GetStudentsWithPagingQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(
        GetStudentsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Students
            .AsNoTracking()
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm)
            .MapToStudentDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}