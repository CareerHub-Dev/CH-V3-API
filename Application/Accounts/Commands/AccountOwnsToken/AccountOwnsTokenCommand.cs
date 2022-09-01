using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.AccountOwnsToken;

public record AccountOwnsTokenCommand : IRequest<bool>
{
    public Guid AccountId { get; init; }
    public string Token { get; init; } = string.Empty;
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
        if (!await _context.Accounts.AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return await _context.RefreshTokens
                .AnyAsync(x => x.Token == request.Token && x.AccountId == request.AccountId);
    }
}
