using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.CreateStudentLog;

public record CreateStudentLogCommand : IRequest<Guid>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid StudentGroupId { get; init; }
}

public class CreateStudentLogCommandHandler : IRequestHandler<CreateStudentLogCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStudentLogCommand request, CancellationToken cancellationToken)
    {
        if(!await _context.StudentGroups.AnyAsync(x => x.Id == request.StudentGroupId))
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        var studentLog = new StudentLog
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            StudentGroupId = request.StudentGroupId
        };

        await _context.StudentLogs.AddAsync(studentLog);

        await _context.SaveChangesAsync(cancellationToken);

        return studentLog.Id;
    }
}
