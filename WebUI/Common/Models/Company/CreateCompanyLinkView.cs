namespace WebUI.Common.Models.Company;

public record CreateCompanyLinkView
{
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
