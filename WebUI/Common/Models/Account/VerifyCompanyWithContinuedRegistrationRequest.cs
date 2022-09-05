namespace WebUI.Common.Models.Account;

public class VerifyCompanyWithContinuedRegistrationRequest
{
    public string Token { get; set; } = string.Empty;

    public IFormFile? LogoFile { get; set; }
    public IFormFile? BannerFile { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Motto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
