namespace WebUI.Common.Models.CompanyLink;

public record UpdateCompanyLinkView
{
    public Guid CompanyLinkId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
