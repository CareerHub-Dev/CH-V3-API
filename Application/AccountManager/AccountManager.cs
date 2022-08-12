using Application.AccountManager.Models;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountManager;

public class AccountManager : IAccountManager
{
    private readonly IApplicationDbContext _context;

    public AccountManager(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsInRoleAsync(Guid accountId, string role)
    {
        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Guid.Empty is not a valid accountId.", nameof(accountId));
        }
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(role));
        }

        var normalizedRole = role.NormalizeName();

        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
        {
            return false;
        }

        var accountNormalizedRole = account.GetType().Name.NormalizeName();

        return accountNormalizedRole == normalizedRole;
    }

    public async Task<AccountInfo?> GetAccountInfoAsync(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Guid.Empty is not a valid accountId.", nameof(accountId));
        }
        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
        {
            return null;
        }

        return new AccountInfo { Id = accountId, Role = account.GetType().Name };
    }
}
