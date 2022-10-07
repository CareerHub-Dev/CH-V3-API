using System.ComponentModel;

namespace API.DTO.Requests.RefreshTokens;

public class RevokeRefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; } = string.Empty;
}
