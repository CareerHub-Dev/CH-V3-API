namespace WebUI.DTO.Account;

public class ResetPasswordRequest : IValidatableMarker
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
