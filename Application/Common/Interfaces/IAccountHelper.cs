using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAccountHelper
{
    string GetRole(Account account);
    void RemoveOldRefreshTokensOfAccount(Account account);
    Task<string> GenerateUniqueResetTokenAsync(CancellationToken cancellationToken = default);
    Task<string> GenerateUniqueVerificationTokenAsync(CancellationToken cancellationToken = default);
}
