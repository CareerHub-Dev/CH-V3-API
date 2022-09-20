using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupsWithPaginationWithSearchQuery : IRequest<PaginatedList<StudentGroupDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }
}

public class GetStudentGroupsWithPaginationWithSearchQueryHandler : IRequestHandler<GetStudentGroupsWithPaginationWithSearchQuery, PaginatedList<StudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupsWithPaginationWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentGroupDTO>> Handle(GetStudentGroupsWithPaginationWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}