namespace WebUI.DTO.Auth;

public class ForgotPasswordRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
