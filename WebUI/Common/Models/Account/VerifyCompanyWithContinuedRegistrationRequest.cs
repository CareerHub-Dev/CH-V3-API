namespace WebUI.Common.Models.Account;

public class VerifyCompanyWithContinuedRegistrationRequest : IValidatableMarker
{
    public string Token { get; set; } = string.Empty;

    public IFormFile? CompanyLogoFile { get; set; }
    public IFormFile? CompanyBannerFile { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string CompanyMotto { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
