namespace WebUI.DTO.Auth;

public class ResetPasswordRequest : IValidatableMarker
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
