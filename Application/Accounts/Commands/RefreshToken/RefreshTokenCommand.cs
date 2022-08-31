using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RefreshTokenEntity = Domain.Entities.RefreshToken;

namespace Application.Accounts.Commands.RefreshToken;

public record RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string Token { init; get; } = string.Empty;
    public string IpAddress { init; get; } = string.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token), cancellationToken);

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
            RevokeDescendantRefreshTokens(refreshToken, account, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.Token}");
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active");
        }

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, request.IpAddress, cancellationToken);
        account.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from account
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtSettings.RefreshTokenTTL) <= DateTime.UtcNow);

        // generate new jwt
        var jwtToken = _jwtService.GenerateJwtToken(account.Id);

        await _context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = newRefreshToken.Token,
            AccountId = account.Id,
            Role = account.GetType().Name
        };
    }

    private void RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, Account account, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = account.RefreshTokens.Single(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken.IsActive)
                RevokeRefreshToken(childToken, ipAddress, reason);
            else
                RevokeDescendantRefreshTokens(childToken, account, ipAddress, reason);
        }
    }

    private void RevokeRefreshToken(RefreshTokenEntity token, string ipAddress, string reason = "", string replacedByToken = "")
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    private async Task<RefreshTokenEntity> RotateRefreshTokenAsync(RefreshTokenEntity refreshToken, string ipAddress, CancellationToken cancellationToken)
    {
        var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(ipAddress, cancellationToken);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
}