using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.AccountOwnsRefreshTokenWithFilter;

public record AccountOwnsRefreshTokenWithFilterQuery : IRequest<bool>
{
    public Guid AccountId { get; init; }
    public bool IsAccountVerified { get; init; }

    public string Token { get; init; } = string.Empty;
}

public class AccountOwnsRefreshTokenCommandHandler : IRequestHandler<AccountOwnsRefreshTokenWithFilterQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public AccountOwnsRefreshTokenCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AccountOwnsRefreshTokenWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .Filter(isVerified: request.IsAccountVerified)
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return await _context.RefreshTokens
                .AnyAsync(x => x.Token == request.Token && x.AccountId == request.AccountId);
    }
}
