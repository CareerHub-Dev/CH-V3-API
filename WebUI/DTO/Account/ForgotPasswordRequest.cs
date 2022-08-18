namespace WebUI.DTO.Account;

public class ForgotPasswordRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
