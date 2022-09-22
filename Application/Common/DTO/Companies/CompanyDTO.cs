namespace Application.Common.DTO.Companies;

public class CompanyDTO : DetailedCompanyDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
