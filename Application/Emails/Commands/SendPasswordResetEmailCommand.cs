using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

    public SendPasswordResetEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, ITemplateService templateService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
    }

    public async Task<Unit> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.NormalizeName(), cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Account), request.Email);
        }

        entity.ResetToken = await GenerateVerificationTokenAsync();
        entity.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.PasswordResetEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{resetToken}", entity.ResetToken ?? "" } });

        await _emailService.SendEmailAsync(entity.NormalizedEmail, "Reset Password", template);

        return Unit.Value;
    }

    private async Task<string> GenerateVerificationTokenAsync()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        // ensure token is unique by checking against db
        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.VerificationToken == token);
        if (!tokenIsUnique)
            return await GenerateVerificationTokenAsync();

        return token;
    }
}
