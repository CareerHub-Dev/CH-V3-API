using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendInviteCompanyEmailCommand(Guid CompanyId) : IRequest;

public class SendInviteCompanyEmailCommandHandler : IRequestHandler<SendInviteCompanyEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;
    private readonly IProcedureService _procedureService;

    public SendInviteCompanyEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, ITemplateService templateService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _templateService = templateService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendInviteCompanyEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (entity.IsVerified)
        {
            throw new ArgumentException("Company is verified");
        }

        entity.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync();
        await _context.SaveChangesAsync(cancellationToken);

        var template = await _templateService.GetTemplateAsync(TemplateConstants.CompanyInvitationEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{verificationToken}", entity.VerificationToken ?? "" } });

        await _emailService.SendEmailAsync(entity.NormalizedEmail, "Invitation Email", template);

        return Unit.Value;
    }
}
