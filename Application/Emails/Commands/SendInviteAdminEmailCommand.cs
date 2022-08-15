using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Emails.Commands;

public record SendInviteAdminEmailCommand(Guid adminId) : IRequest;

public class SendInviteAdminEmailCommandHandler : IRequestHandler<SendInviteAdminEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;

    public SendInviteAdminEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, ITemplateService templateService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
    }

    public async Task<Unit> Handle(SendInviteAdminEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins.FirstOrDefaultAsync(x => x.Id == request.adminId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.adminId);
        }

        if (entity.IsVerified)
        {
            throw new ArgumentException("Admin is verified");
        }

        entity.VerificationToken = await GenerateVerificationTokenAsync();
        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.AdminInvitationEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{verificationToken}", entity.VerificationToken ?? "" } });

        await _emailService.SendEmailAsync(entity.NormalizedEmail, "Invitation Email", template);

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