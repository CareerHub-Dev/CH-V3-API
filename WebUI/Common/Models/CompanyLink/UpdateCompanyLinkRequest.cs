namespace WebUI.Common.Models.CompanyLink;

public class UpdateCompanyLinkRequest : IValidatableMarker
{
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}
