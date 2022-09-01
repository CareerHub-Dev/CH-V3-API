using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendInviteAdminEmailCommand(Guid AdminId) : IRequest;

public class SendInviteAdminEmailCommandHandler : IRequestHandler<SendInviteAdminEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IProcedureService _procedureService;

    public SendInviteAdminEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendInviteAdminEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins.FirstOrDefaultAsync(x => x.Id == request.AdminId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        if (entity.IsVerified)
        {
            throw new ArgumentException("Admin is verified");
        }

        entity.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendInviteAdminEmailAsync(entity);

        return Unit.Value;
    }
}