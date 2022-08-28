using Application.Accounts.Event;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RegisterStudent;

public record RegisterStudentCommand : IRequest
{
    public string Email { init; get; } = string.Empty;
    public string Password { init; get; } = string.Empty;
}

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public RegisterStudentCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var studentLog = await _context.StudentLogs
                .AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.NormalizedEmail == request.Email.NormalizeName(), cancellationToken
                );

        if (studentLog == null)
        {
            if (await _context.Students.AnyAsync(x => x.NormalizedEmail == request.Email.NormalizeName(), cancellationToken))
            {
                throw new ArgumentException("This email address is already being used.");
            }

            throw new NotFoundException(nameof(StudentLog), request.Email);
        }

        var entity = new Student
        {
            Email = studentLog.Email,
            NormalizedEmail = studentLog.NormalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Verified = null,
            FirstName = studentLog.FirstName,
            LastName = studentLog.LastName,
            StudentGroupId = studentLog.StudentGroupId,
        };

        await _context.Students.AddAsync(entity, cancellationToken);
        _context.StudentLogs.Remove(studentLog);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new StudentRegisteredEvent(entity));

        return Unit.Value;
    }
}