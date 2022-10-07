using System.ComponentModel;

namespace API.DTO.Requests.RefreshTokens;

public class RefreshTokenRequest
{
    [DefaultValue("")]
    public string? Token { get; set; }
}
