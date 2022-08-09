namespace Application.Common.Interfaces;

public interface IAccountManager
{
    Task<bool> IsInRoleAsync(Guid accountId, string role);
}
