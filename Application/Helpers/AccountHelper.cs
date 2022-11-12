using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Application.Helpers;

public class AccountHelper : IAccountHelper
{
    private readonly JwtSettings _jwtSettings;
    private readonly IApplicationDbContext _context;

    public AccountHelper(
        IOptions<JwtSettings> jwtSettings,
        IApplicationDbContext context)
    {
        _jwtSettings = jwtSettings.Value;
        _context = context;
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

    public void RemoveOldRefreshTokensOfAccount(Account account)
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
