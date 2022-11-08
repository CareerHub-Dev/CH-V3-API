using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RefreshTokenEntity = Domain.Entities.RefreshToken;

namespace Application.Accounts.Queries.RefreshToken;

public record RefreshTokenQuery : IRequest<RefreshTokenResult>
{
    public string Token { init; get; } = string.Empty;
}

public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, RefreshTokenResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    private readonly IAccountHelper _accountHelper;
    private readonly IRefreshTokenHelper _refreshTokenHelper;

    public RefreshTokenQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService,
        IAccountHelper accountHelper,
        IRefreshTokenHelper refreshTokenHelper)
    {
        _context = context;
        _jwtService = jwtService;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
        _accountHelper = accountHelper;
        _refreshTokenHelper = refreshTokenHelper;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .Include(x => x.RefreshTokens)
                .Filter(isVerified: true)
                .SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == request.Token));

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.Token);
        }

        var refreshToken = account.RefreshTokens.Single(x => x.Token == request.Token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            _refreshTokenHelper.RevokeDescendantRefreshTokens(refreshToken, account, _сurrentRemoteIpAddressService.IpAddress, $"Attempted reuse of revoked ancestor token: {request.Token}");
            await _context.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active.");
        }

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, _сurrentRemoteIpAddressService.IpAddress);
        account.RefreshTokens.Add(newRefreshToken);

        _accountHelper.RemoveOldRefreshTokensOfAccount(account);

        // generate new jwt
        var jwtToken = _jwtService.GenerateJwtToken(account.Id);

        await _context.SaveChangesAsync();

        return new RefreshTokenResult
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpires = newRefreshToken.Expires,
            AccountId = account.Id,
            Role = _accountHelper.GetRole(account)
        };
    }

    private async Task<RefreshTokenEntity> RotateRefreshTokenAsync(RefreshTokenEntity refreshToken, string ipAddress)
    {
        var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(ipAddress);
        _refreshTokenHelper.RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
}