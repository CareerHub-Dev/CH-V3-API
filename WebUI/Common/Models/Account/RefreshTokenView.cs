using System.ComponentModel;

namespace WebUI.Common.Models.Account;

public class RefreshTokenView
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
