namespace WebUI.Common.Models.Company;

public class UpdateCompanyRequest : IValidatableMarker
{
    public string CompanyName { get; init; } = string.Empty;
    public string CompanyMotto { get; init; } = string.Empty;
    public string CompanyDescription { get; init; } = string.Empty;
}
