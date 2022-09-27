using Application.Accounts.Event;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    private readonly IPasswordHasher<Account> _passwordHasher;

    public RegisterStudentCommandHandler(IApplicationDbContext context, IMediator mediator, IPasswordHasher<Account> passwordHasher)
    {
        _context = context;
        _mediator = mediator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var studentLog = await _context.StudentLogs
            .AsNoTracking()
            .SingleOrDefaultAsync(x =>
                x.NormalizedEmail == request.Email.NormalizeName()
            );

        if (studentLog == null)
        {
            if (await _context.Accounts.AnyAsync(x => x.NormalizedEmail == request.Email.NormalizeName()))
            {
                throw new ArgumentException("This email address is already being used.");
            }

            throw new NotFoundException(nameof(StudentLog), request.Email);
        }

        var student = new Student
        {
            Email = studentLog.Email,
            Verified = null,
            FirstName = studentLog.FirstName,
            LastName = studentLog.LastName,
            StudentGroupId = studentLog.StudentGroupId,
            ActivationStatus = ActivationStatus.Active,
        };

        student.PasswordHash = _passwordHasher.HashPassword(student, request.Password);

        await _context.Students.AddAsync(student);
        _context.StudentLogs.Remove(studentLog);
        await _context.SaveChangesAsync();

        await _mediator.Publish(new StudentRegisteredEvent(student));

        return Unit.Value;
    }
}