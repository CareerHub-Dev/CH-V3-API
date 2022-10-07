using System.ComponentModel;

namespace WebUI.DTO.Requests.RefreshTokens;

public class RevokeRefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; } = string.Empty;
}
