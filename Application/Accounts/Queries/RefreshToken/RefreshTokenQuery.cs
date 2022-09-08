using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    private readonly JwtSettings _jwtSettings;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;

    public RefreshTokenQueryHandler(
        IApplicationDbContext context, 
        IJwtService jwtService, 
        IOptions<JwtSettings> jwtSettings, 
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService)
    {
        _context = context;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.Token);
        }

        if (!account.IsVerified)
        {
            throw new ArgumentException("Account is not Verified.");
        }

        var refreshToken = account.RefreshTokens.Single(x => x.Token == request.Token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            RefreshTokenHelper.RevokeDescendantRefreshTokens(refreshToken, account, _сurrentRemoteIpAddressService.IpAddress, $"Attempted reuse of revoked ancestor token: {request.Token}");
            await _context.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active.");
        }

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, _сurrentRemoteIpAddressService.IpAddress);
        account.RefreshTokens.Add(newRefreshToken);

        AccountHelper.RemoveOldRefreshTokens(account, _jwtSettings.RefreshTokenTTL);

        // generate new jwt
        var jwtToken = _jwtService.GenerateJwtToken(account.Id);

        await _context.SaveChangesAsync();

        return new RefreshTokenResult
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = newRefreshToken.Token,
            AccountId = account.Id,
            Role = AccountHelper.GetRole(account)
        };
    }

    private async Task<RefreshTokenEntity> RotateRefreshTokenAsync(RefreshTokenEntity refreshToken, string ipAddress)
    {
        var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(ipAddress);
        RefreshTokenHelper.RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
}