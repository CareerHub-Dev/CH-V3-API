using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.DeleteStudentLog;

public record DeleteStudentLogCommand(Guid StudentLogId)
    : IRequest;

public class DeleteStudentLogCommandHandler
    : IRequestHandler<DeleteStudentLogCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentLogCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteStudentLogCommand request, 
        CancellationToken cancellationToken)
    {
        var studentLog = await _context.StudentLogs
            .Where(x => x.Id == request.StudentLogId)
            .FirstOrDefaultAsync();

        if (studentLog == null)
        {
            throw new NotFoundException(nameof(StudentLog), request.StudentLogId);
        }

        _context.StudentLogs.Remove(studentLog);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
