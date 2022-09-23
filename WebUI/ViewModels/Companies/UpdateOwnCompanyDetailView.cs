namespace WebUI.ViewModels.Companies;

public record UpdateOwnCompanyDetailView
{
    public string Name { get; init; } = string.Empty;
    public string Motto { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
