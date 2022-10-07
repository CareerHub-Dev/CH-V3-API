namespace API.DTO.Requests.CompanyLinks;

public record UpdateCompanyLinkRequest
{
    public Guid CompanyLinkId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
