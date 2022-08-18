using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Emails.Commands;

public record SendVerifyStudentEmailCommand(Guid studentId) : IRequest;

public class SendVerifyStudentEmailCommandHandler : IRequestHandler<SendVerifyStudentEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;
    private readonly IProcedureService _procedureService;

    public SendVerifyStudentEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, ITemplateService templateService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendVerifyStudentEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students.FirstOrDefaultAsync(x => x.Id == request.studentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.studentId);
        }

        if(entity.IsVerified)
        {
            throw new ArgumentException("Student is verified");
        }

        entity.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync();
        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.VerifyStudentEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{verificationToken}", entity.VerificationToken ?? "" } });

        await _emailService.SendEmailAsync(entity.NormalizedEmail, "Student Verification", template);

        return Unit.Value;
    }
}
