namespace WebUI.Common.Models.Company;

public class UpdateCompanyLogoRequest : IValidatableMarker
{
    public IFormFile? LogoFile { get; set; }
}
