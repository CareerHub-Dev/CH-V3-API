using System.ComponentModel;

namespace WebUI.ViewModels.RefreshTokens;

public class RevokeRefreshTokenView
{
    [DefaultValue("")]
    public string? Token { get; set; } = string.Empty;
}
