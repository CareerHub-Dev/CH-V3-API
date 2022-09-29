using Domain.Enums;

namespace Application.Common.DTO.Companies;

public class CompanyDTO : DetailedCompanyDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public ActivationStatus ActivationStatus { get; set; }
}
