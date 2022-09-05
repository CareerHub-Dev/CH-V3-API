using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Common.Models.StudentGroup;
using Application.StudentLogs.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Queries;

public record GetStudentLogsWithPaginationWithSearchWithFilterQuery : IRequest<PaginatedList<StudentLogDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public List<Guid>? StudentGroupIds { get; init; }
}

public class GetStudentLogsWithPaginationWithSearchWithFilterQueryHandler : IRequestHandler<GetStudentLogsWithPaginationWithSearchWithFilterQuery, PaginatedList<StudentLogDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentLogDTO>> Handle(GetStudentLogsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentLogs
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .Filter(request.StudentGroupIds)
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            .Select(x => new StudentLogDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Created = x.Created,
                LastModified = x.LastModified,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
                StudentGroup = new StudentGroupBriefDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name }
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}