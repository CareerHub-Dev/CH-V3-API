using Application.Common.DTO.StudentLogs;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Queries.GetStudentLogs;

public record GetStudentLogsWithPaginationWithSearchWithFilterWithSortQuery : IRequest<PaginatedList<StudentLogDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentLogsWithPaginationWithSearchWithFilterWithSortQueryHandler : IRequestHandler<GetStudentLogsWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<StudentLogDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogsWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentLogDTO>> Handle(GetStudentLogsWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentLogs
            .AsNoTracking()
            .Search(request.SearchTerm)
            .Filter(studentGroupIds: request.StudentGroupIds)
            .MapToStudentLogDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}