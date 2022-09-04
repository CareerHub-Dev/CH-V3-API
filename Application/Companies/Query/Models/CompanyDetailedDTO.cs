namespace Application.Companies.Query.Models;

public class CompanyDetailedDTO : CompanyBriefDTO
{
    public string Motto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
