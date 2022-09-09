using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRefreshTokenHelper
{
    void RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string reason = "", string replacedByToken = "");
    void RevokeDescendantRefreshTokens(RefreshToken refreshToken, Account account, string ipAddress, string reason);
}
