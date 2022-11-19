using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands.SendInviteAdminEmail;

public record SendInviteAdminEmailCommand(Guid AdminId)
    : IRequest;

public class SendInviteAdminEmailCommandHandler
    : IRequestHandler<SendInviteAdminEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IAccountHelper _accountHelper;
    public SendInviteAdminEmailCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IAccountHelper accountHelper)
    {
        _context = context;
        _emailService = emailService;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(
        SendInviteAdminEmailCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(x => x.Id == request.AdminId);

        if (admin == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        if (admin.IsVerified)
        {
            throw new ArgumentException("Admin is verified");
        }

        admin.VerificationToken = await _accountHelper.GenerateUniqueVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendInviteAdminEmailAsync(admin);

        return Unit.Value;
    }
}