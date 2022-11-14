namespace Application.Common.DTO.Companies;

public class BriefCompanyDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? Banner { get; set; }
}
