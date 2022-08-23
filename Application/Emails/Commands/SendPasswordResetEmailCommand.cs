using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendPasswordResetEmailCommand : IRequest
{
    public string Email { get; init; } = string.Empty;
}

public class SendPasswordResetEmailCommandHandler : IRequestHandler<SendPasswordResetEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;
    private readonly IProcedureService _procedureService;

    public SendPasswordResetEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, ITemplateService templateService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.NormalizeName(), cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Account), request.Email);
        }

        entity.ResetToken = await _procedureService.GenerateAccountResetTokenAsync(cancellationToken);
        entity.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.PasswordResetEmail, cancellationToken);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{resetToken}", entity.ResetToken ?? "" } });

        await _emailService.SendEmailAsync(entity.NormalizedEmail, "Reset Password", template, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
