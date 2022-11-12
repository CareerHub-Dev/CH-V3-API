using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
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
    private readonly IAccountHelper _accountHelper;

    public SendPasswordResetEmailCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IAccountHelper accountHelper)
    {
        _context = context;
        _emailService = emailService;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.NormalizeName());

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.Email);
        }

        account.ResetToken = await _accountHelper.GenerateUniqueResetTokenAsync();
        account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

        await _context.SaveChangesAsync();

        await _emailService.SendPasswordResetEmailAsync(account);

        return Unit.Value;
    }
}
