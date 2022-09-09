using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAccountHelper
{
    string GetRole(Account account);
    void RemoveOldRefreshTokensOfAccount(Account account);
}
