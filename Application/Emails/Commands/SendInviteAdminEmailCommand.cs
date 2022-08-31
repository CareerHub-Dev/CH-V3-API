using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendInviteAdminEmailCommand(Guid AdminId) : IRequest;

public class SendInviteAdminEmailCommandHandler : IRequestHandler<SendInviteAdminEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMailKitService _emailService;
    private readonly ITemplateService _templateService;
    private readonly IProcedureService _procedureService;

    public SendInviteAdminEmailCommandHandler(IApplicationDbContext context, IMailKitService emailService, ITemplateService templateService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendInviteAdminEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins.FirstOrDefaultAsync(x => x.Id == request.AdminId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        if (entity.IsVerified)
        {
            throw new ArgumentException("Admin is verified");
        }

        entity.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.AdminInvitationEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{verificationToken}", entity.VerificationToken ?? "" } });

        await _emailService.SendAsync(entity.NormalizedEmail, "Invitation Email", template);

        return Unit.Value;
    }
}