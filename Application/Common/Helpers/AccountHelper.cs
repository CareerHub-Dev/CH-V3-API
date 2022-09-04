using Domain.Entities;

namespace Application.Common.Helpers;

public static class AccountHelper
{
    public static string GetRole(Account account)
    {
        var admin = account as Admin;

        if(admin == null)
        {
            return account.GetType().Name;
        }

        return admin.IsSuperAdmin ? "SuperAdmin" : "Admin";
    }

    public static void RemoveOldRefreshTokens(Account account, int refreshTokenTTL)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(refreshTokenTTL) <= DateTime.UtcNow);
    }
}
