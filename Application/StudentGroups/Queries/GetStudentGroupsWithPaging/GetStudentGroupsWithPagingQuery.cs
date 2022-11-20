using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;

namespace Application.StudentGroups.Queries.GetStudentGroupsWithPaging;

public record GetStudentGroupsWithPagingQuery
    : IRequest<PaginatedList<StudentGroupDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentGroupsWithPagingQueryHandler
    : IRequestHandler<GetStudentGroupsWithPagingQuery, PaginatedList<StudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentGroupDTO>> Handle(
        GetStudentGroupsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .Search(request.SearchTerm)
            .MapToStudentGroupDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}