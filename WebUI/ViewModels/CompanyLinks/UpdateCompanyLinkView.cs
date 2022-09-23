namespace WebUI.ViewModels.CompanyLinks;

public record UpdateCompanyLinkView
{
    public Guid CompanyLinkId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
