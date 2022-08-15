namespace WebUI.DTO.Auth;

public class VerifyCompanyWithContinuedRegistrationRequest : IValidatableMarker
{
    public string Token { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;
    public string CompanyMotto { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
