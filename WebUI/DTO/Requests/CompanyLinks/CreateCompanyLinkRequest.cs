namespace WebUI.DTO.Requests.CompanyLinks;

public record CreateCompanyLinkRequest
{
    public string Title { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
