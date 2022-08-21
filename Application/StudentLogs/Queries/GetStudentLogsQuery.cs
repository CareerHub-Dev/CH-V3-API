using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.StudentGroups.Queries.Models;
using Application.StudentLogs.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Queries;

public record GetStudentLogsQuery : IRequest<PaginatedList<StudentLogDTO>>
{
    public PaginationParameters PaginationParameters { get; init; } = new PaginationParameters();
}

public class GetStudentLogsQueryHandler : IRequestHandler<GetStudentLogsQuery, PaginatedList<StudentLogDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentLogDTO>> Handle(GetStudentLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.StudentLogs
                .AsNoTracking();

        return await query
            .Select(x => new StudentLogDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                StudentGroup = new StudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name }
            })
            .ToPagedListAsync(request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize, cancellationToken);
    }
}