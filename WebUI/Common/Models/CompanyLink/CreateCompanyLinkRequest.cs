namespace WebUI.Common.Models.CompanyLink;

public class CreateCompanyLinkRequest : IValidatableMarker
{
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}
