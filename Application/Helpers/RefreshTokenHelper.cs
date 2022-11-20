using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Helpers;

public class RefreshTokenHelper : IRefreshTokenHelper
{
    public void RevokeRefreshToken(
        RefreshToken refreshToken,
        string ipAddress,
        string reason = "",
        string replacedByToken = "")
    {
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
    }

    public void RevokeDescendantRefreshTokens(
        RefreshToken refreshToken,
        Account account,
        string ipAddress,
        string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = account.RefreshTokens
                .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

            if (childToken != null)
            {
                if (childToken.IsActive)
                {
                    RevokeRefreshToken(childToken, ipAddress, reason);
                }
                else
                {
                    RevokeDescendantRefreshTokens(childToken, account, ipAddress, reason);
                }
            }
        }
    }
}
