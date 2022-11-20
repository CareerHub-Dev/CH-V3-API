using Application.Common.DTO.StudentLogs;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;

namespace Application.StudentLogs.Queries.GetStudentLogsWithPaging;

public record GetStudentLogsWithPagingQuery
    : IRequest<PaginatedList<StudentLogDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentLogsWithPagingQueryHandler
    : IRequestHandler<GetStudentLogsWithPagingQuery, PaginatedList<StudentLogDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentLogDTO>> Handle(
        GetStudentLogsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.StudentLogs
            .Search(request.SearchTerm)
            .Filter(studentGroupIds: request.StudentGroupIds)
            .MapToStudentLogDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}