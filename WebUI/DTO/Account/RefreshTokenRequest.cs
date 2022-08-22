using System.ComponentModel;

namespace WebUI.DTO.Account;

public class RefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
