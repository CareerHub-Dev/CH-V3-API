using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries.GetStudentGroups;

public record GetStudentGroupsWithPaginationWithSearchWithSortQuery : IRequest<PaginatedList<StudentGroupDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentGroupsWithPaginationWithSearchWithSortQueryHandler : IRequestHandler<GetStudentGroupsWithPaginationWithSearchWithSortQuery, PaginatedList<StudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupsWithPaginationWithSearchWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentGroupDTO>> Handle(GetStudentGroupsWithPaginationWithSearchWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .AsNoTracking()
            .Search(request.SearchTerm)
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}