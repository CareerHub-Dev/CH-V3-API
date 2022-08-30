namespace WebUI.Common.Models.Company;

public class UpdateCompanyBannerRequest : IValidatableMarker
{
    public IFormFile? BannerFile { get; set; }
}
