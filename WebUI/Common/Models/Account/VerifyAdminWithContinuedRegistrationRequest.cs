namespace WebUI.Common.Models.Account;

public class VerifyAdminWithContinuedRegistrationRequest : IValidatableMarker
{
    public string Token { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
