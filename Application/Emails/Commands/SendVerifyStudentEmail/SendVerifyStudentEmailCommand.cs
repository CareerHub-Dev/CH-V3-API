using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands.SendVerifyStudentEmail;

public record SendVerifyStudentEmailCommand(Guid StudentId)
    : IRequest;

public class SendVerifyStudentEmailCommandHandler
    : IRequestHandler<SendVerifyStudentEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IAccountHelper _accountHelper;

    public SendVerifyStudentEmailCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IAccountHelper accountHelper)
    {
        _context = context;
        _emailService = emailService;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(
        SendVerifyStudentEmailCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Where(x => x.Id == request.StudentId)
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if (student.IsVerified)
        {
            throw new ArgumentException("Student is verified");
        }

        student.VerificationToken = await _accountHelper.GenerateUniqueVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendVerifyStudentEmailAsync(student);

        return Unit.Value;
    }
}
