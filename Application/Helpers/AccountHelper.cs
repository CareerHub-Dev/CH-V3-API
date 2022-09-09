using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Helpers;

public class AccountHelper : IAccountHelper
{
    private readonly JwtSettings _jwtSettings;

    public AccountHelper(
        IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GetRole(Account account)
    {
        var admin = account as Admin;

        if (admin == null)
        {
            return account.GetType().Name;
        }

        return admin.IsSuperAdmin ? "SuperAdmin" : "Admin";
    }

    public void RemoveOldRefreshTokensOfAccount(Account account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }
}
