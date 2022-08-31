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
    private readonly IProcedureService _procedureService;

    public SendPasswordResetEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
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

        await _emailService.SendPasswordResetEmailAsync(entity);

        return Unit.Value;
    }
}
