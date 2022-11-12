using Application.Common.Entensions;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bans.Commands.CreateBan;

public record CreateBanCommand : IRequest<Guid>
{
    public Guid AccountId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime Expires { get; init; }
}

public class CreateBanCommandHandler : IRequestHandler<CreateBanCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccountHelper _accountHelper;

    public CreateBanCommandHandler(IApplicationDbContext context, IAccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<Guid> Handle(CreateBanCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
           .AsNoTracking()
           .Filter(isVerified: true)
           .Where(x => x.Id == request.AccountId)
           .FirstOrDefaultAsync();

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        if (_accountHelper.GetRole(account) == Role.SuperAdmin)
        {
            throw new ForbiddenException($"It is forbidden to ban the {Role.SuperAdmin.ToString()}.");
        }

        var ban = new Ban
        {
            Reason = request.Reason,
            Expires = request.Expires,
            AccountId = account.Id,
        };

        await _context.Bans.AddAsync(ban);

        await _context.SaveChangesAsync();

        return ban.Id;
    }
}
