using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.ReturnStudentToLog;

public record ReturnStudentToLogCommand(Guid StudentId)
    : IRequest;

public class ReturnStudentToLogCommandHandler
    : IRequestHandler<ReturnStudentToLogCommand>
{
    private readonly IApplicationDbContext _context;

    public ReturnStudentToLogCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        ReturnStudentToLogCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        _context.Students.Remove(student);

        var studentLog = new StudentLog
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentGroupId = student.StudentGroupId
        };

        await _context.StudentLogs.AddAsync(studentLog);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}