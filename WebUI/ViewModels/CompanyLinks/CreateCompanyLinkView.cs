namespace WebUI.ViewModels.CompanyLinks;

public record CreateCompanyLinkView
{
    public string Title { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
