using System.ComponentModel;

namespace WebUI.DTO.Requests.RefreshTokens;

public class RefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
