using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.StudentGroups.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupsWithPaginationWithSearchWithFilterQuery : IRequest<PaginatedList<StudentGroupDTO>>
{
    public PaginationParameters PaginationParameters { get; init; } = new PaginationParameters();

    public string? SearchTerm { get; init; }

    public Guid? CreatedBy { get; init; }
    public Guid? LastModifiedBy { get; init; }
}

public class GetStudentGroupsWithPaginationWithSearchWithFilterQueryHandler : IRequestHandler<GetStudentGroupsWithPaginationWithSearchWithFilterQuery, PaginatedList<StudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentGroupDTO>> Handle(GetStudentGroupsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .Filter(request.CreatedBy, request.LastModifiedBy)
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToPagedListAsync(request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize, cancellationToken);
    }
}