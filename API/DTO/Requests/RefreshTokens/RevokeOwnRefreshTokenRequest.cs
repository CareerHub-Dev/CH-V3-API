using System.ComponentModel;

namespace API.DTO.Requests.RefreshTokens;

public class RevokeOwnRefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; } = string.Empty;
}
