using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IAccountManager
{
    Task<bool> IsInRoleAsync(Guid accountId, string role);
    Task<AccountInfo?> GetAccountInfoAsync(Guid accountId);
}
