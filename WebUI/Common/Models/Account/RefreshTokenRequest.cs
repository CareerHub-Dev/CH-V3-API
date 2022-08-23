using System.ComponentModel;

namespace WebUI.Common.Models.Account;

public class RefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
