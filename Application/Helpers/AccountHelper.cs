using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace Application.Helpers;

public class AccountHelper : IAccountHelper
{
    private readonly JwtSettings _jwtSettings;
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountHelper(
        IOptions<JwtSettings> jwtSettings,
        IApplicationDbContext context,
        IPasswordHasher<Account> passwordHasher)
    {
        _jwtSettings = jwtSettings.Value;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public Role GetRole(Account account)
    {
        switch (account)
        {
            case Company:
                return Role.Company;
            case Student:
                return Role.Student;
            case Admin admin when admin.IsSuperAdmin:
                return Role.SuperAdmin;
            case Admin:
                return Role.Admin;
            default:
                return Role.None;
        }
    }

    public void VerifyPasswordWithRehashIfNeed(Account account, string password)
    {
        var passwordVerificationResult = _passwordHasher
            .VerifyHashedPassword(account, account.PasswordHash, password);

        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                throw new AuthenticationException("This combination of email and password doesn't exist");

            case PasswordVerificationResult.SuccessRehashNeeded:
                account.PasswordHash = _passwordHasher.HashPassword(account, password);
                break;
        }
    }

    public void ThrowIfBanned(Account account)
    {
        var ban = account.Bans
            .Where(x => x.Expires >= DateTime.UtcNow)
            .OrderBy(x => x.Expires)
            .LastOrDefault();

        if (ban != null)
        {
            throw new BanException(ban.Reason, ban.Expires);
        }
    }

    public bool IsBanned(Account account)
    {
        return account.Bans.Any(x => x.Expires >= DateTime.UtcNow);
    }

    public void RemoveOldRefreshTokens(Account account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    public async Task<string> GenerateUniqueResetTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.ResetToken == token, cancellationToken);
        if (!tokenIsUnique)
            return await GenerateUniqueResetTokenAsync(cancellationToken);

        return token;
    }

    public async Task<string> GenerateUniqueVerificationTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.VerificationToken == token, cancellationToken);
        if (!tokenIsUnique)
            return await GenerateUniqueVerificationTokenAsync(cancellationToken);

        return token;
    }
}
