using Application.Common.Enums;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAccountHelper
{
    Role GetRole(Account account);
    void RemoveOldRefreshTokensOfAccount(Account account);
    Task<string> GenerateUniqueResetTokenAsync(CancellationToken cancellationToken = default);
    Task<string> GenerateUniqueVerificationTokenAsync(CancellationToken cancellationToken = default);
}
