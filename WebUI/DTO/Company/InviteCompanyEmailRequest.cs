namespace WebUI.DTO.Company;

public class InviteCompanyEmailRequest : IValidatableMarker
{
    public Guid CompanyId { get; set; }
}
