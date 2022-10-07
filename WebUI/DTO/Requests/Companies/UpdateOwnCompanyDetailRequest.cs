namespace WebUI.DTO.Requests.Companies;

public record UpdateOwnCompanyDetailRequest
{
    public string Name { get; init; } = string.Empty;
    public string Motto { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
