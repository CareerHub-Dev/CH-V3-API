using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendVerifyStudentEmailCommand(Guid StudentId) : IRequest;

public class SendVerifyStudentEmailCommandHandler : IRequestHandler<SendVerifyStudentEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IProcedureService _procedureService;

    public SendVerifyStudentEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendVerifyStudentEmailCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if (student.IsVerified)
        {
            throw new ArgumentException("Student is verified");
        }

        student.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendVerifyStudentEmailAsync(student);

        return Unit.Value;
    }
}
