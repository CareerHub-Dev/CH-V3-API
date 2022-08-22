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
    public PaginationParameters PaginationParameters { get; init; } = new PaginationParameters();

    public string? SearchTerm { get; init; }

    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? StudentGroupId { get; set; }
}

public class GetStudentLogsQueryHandler : IRequestHandler<GetStudentLogsWithPaginationWithSearchWithFilterQuery, PaginatedList<StudentLogDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentLogDTO>> Handle(GetStudentLogsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentLogs
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .Filter(request.CreatedBy, request.LastModifiedBy, request.StudentGroupId)
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
            .ToPagedListAsync(request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize, cancellationToken);
    }
}