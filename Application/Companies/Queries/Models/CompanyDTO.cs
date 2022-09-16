namespace Application.Companies.Queries.Models;

public class CompanyDTO : DetailedCompanyDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
