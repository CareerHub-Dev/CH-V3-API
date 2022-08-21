using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.AccountOwnsToken;

public record AccountOwnsTokenCommand : IRequest<bool>
{
    public Guid AccountId { get; set; }
    public string Token { get; set; } = string.Empty;
}

public class AccountOwnsTokenCommandHandler : IRequestHandler<AccountOwnsTokenCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AccountOwnsTokenCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AccountOwnsTokenCommand request, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
                .AnyAsync(x => x.Token == request.Token && x.AccountId == request.AccountId, cancellationToken);
    }
}
