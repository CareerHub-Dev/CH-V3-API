using System.ComponentModel;

namespace WebUI.ViewModels.RefreshTokens;

public class RefreshTokenView
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
