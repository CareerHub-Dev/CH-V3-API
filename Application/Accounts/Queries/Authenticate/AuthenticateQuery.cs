using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Application.Accounts.Queries.Authenticate;

public record AuthenticateQuery
    : IRequest<AuthenticateResult>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class AuthenticateQueryHandler
    : IRequestHandler<AuthenticateQuery, AuthenticateResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    private readonly IAccountHelper _accountHelper;
    public AuthenticateQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService,
        IAccountHelper accountHelper)
    {
        _context = context;
        _jwtService = jwtService;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
        _accountHelper = accountHelper;
    }

    public async Task<AuthenticateResult> Handle(
        AuthenticateQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(x => x.RefreshTokens)
            .Include(x => x.Bans)
            .Filter(isVerified: true)
            .SingleOrDefaultAsync(x => x.NormalizedEmail == request.Email.NormalizeName());

        if (account == null)
        {
            throw new AuthenticationException("This combination of email and password doesn't exist");
        }

        _accountHelper.VerifyPasswordWithRehashIfNeed(account, request.Password);

        _accountHelper.ThrowIfBanned(account);

        var jwtToken = _jwtService.GenerateJwtToken(account.Id);
        var refreshToken = await _jwtService.GenerateRefreshTokenAsync(_сurrentRemoteIpAddressService.IpAddress);

        account.RefreshTokens.Add(refreshToken);

        _accountHelper.RemoveOldRefreshTokens(account);

        await _context.SaveChangesAsync();

        return new AuthenticateResult
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = refreshToken.Expires,
            AccountId = account.Id,
            Role = _accountHelper.GetRole(account)
        };
    }
}
