namespace Application.Companies.Queries.Models;

public class CompanyDTO : CompanyDetailedDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
