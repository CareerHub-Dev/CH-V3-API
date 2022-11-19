using Application.Common.DTO.StudentLogs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetStudentLog;

public record GetStudentLogQuery(Guid StudentLogId)
    : IRequest<StudentLogDTO>;

public class GetStudentLogQueryHandler
    : IRequestHandler<GetStudentLogQuery, StudentLogDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentLogDTO> Handle(
        GetStudentLogQuery request,
        CancellationToken cancellationToken)
    {
        var studentGroup = await _context.StudentLogs
            .MapToStudentLogDTO()
            .FirstOrDefaultAsync(x => x.Id == request.StudentLogId);

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentLog), request.StudentLogId);
        }

        return studentGroup;
    }
}