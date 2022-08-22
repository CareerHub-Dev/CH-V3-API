using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.UpdateStudentLog;

public record UpdateStudentLogCommand : IRequest
{
    public Guid StudentLogId { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid StudentGroupId { get; init; }
}

public class UpdateStudentLogCommandHandler : IRequestHandler<UpdateStudentLogCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateStudentLogCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentLogs
            .FirstOrDefaultAsync(x => x.Id == request.StudentLogId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(StudentLog), request.StudentLogId);
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Email = request.Email;
        entity.StudentGroupId = request.StudentGroupId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}