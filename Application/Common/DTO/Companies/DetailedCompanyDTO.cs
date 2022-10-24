namespace Application.Common.DTO.Companies;

public class DetailedCompanyDTO : BriefCompanyDTO
{
    public string Email { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? Banner { get; set; }
    public string Motto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
