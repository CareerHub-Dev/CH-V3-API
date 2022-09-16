using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RevokeRefreshTokenOfAccount;

public record RevokeRefreshTokenOfAccountCommand : IRequest
{
    public string Token { init; get; } = string.Empty;
    public Guid AccountId { get; init; }
}
public class RevokeRefreshTokenOfAccountCommandHandler : IRequestHandler<RevokeRefreshTokenOfAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    public RevokeRefreshTokenOfAccountCommandHandler(
        IApplicationDbContext context, 
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService)
    {
        _context = context;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenOfAccountCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .Filter(isVerified: true)
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var refreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == request.Token && x.AccountId == request.AccountId);

        if (refreshToken == null)
        {
            throw new NotFoundException(nameof(RefreshToken), request.Token);
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active.");
        }

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = _сurrentRemoteIpAddressService.IpAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}