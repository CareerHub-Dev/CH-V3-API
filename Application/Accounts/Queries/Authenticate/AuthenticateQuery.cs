using Application.Common.Entensions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Application.Accounts.Queries.Authenticate;

public record AuthenticateQuery : IRequest<AuthenticateResult>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IAccountHelper _accountHelper;
    public AuthenticateQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService,
        IPasswordHasher<Account> passwordHasher,
        IAccountHelper accountHelper)
    {
        _context = context;
        _jwtService = jwtService;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
        _passwordHasher = passwordHasher;
        _accountHelper = accountHelper;
    }

    public async Task<AuthenticateResult> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync(x =>
                    x.NormalizedEmail == request.Email.NormalizeName()
                );

        if (account == null || !account.IsVerified)
        {
            throw new AuthenticationException("This combination of email and password doesn't exist");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, request.Password);

        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                throw new AuthenticationException("This combination of email and password doesn't exist");

            case PasswordVerificationResult.SuccessRehashNeeded:
                account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);
                break;
        }

        var jwtToken = _jwtService.GenerateJwtToken(account.Id);
        var refreshToken = await _jwtService.GenerateRefreshTokenAsync(_сurrentRemoteIpAddressService.IpAddress);

        account.RefreshTokens.Add(refreshToken);

        _accountHelper.RemoveOldRefreshTokensOfAccount(account);

        await _context.SaveChangesAsync();

        return new AuthenticateResult
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = refreshToken.Token,
            AccountId = account.Id,
            Role = _accountHelper.GetRole(account)
        };
    }
}
