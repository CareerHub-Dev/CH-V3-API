using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.DeleteStudentLog;

public record DeleteStudentLogCommand(Guid StudentLogId) : IRequest;

public class DeleteStudentLogCommandHandler : IRequestHandler<DeleteStudentLogCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteStudentLogCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.StudentLogId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(StudentLog), request.StudentLogId);
        }

        _context.StudentLogs.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
