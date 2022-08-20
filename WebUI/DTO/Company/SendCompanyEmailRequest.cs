namespace WebUI.DTO.Company;

public class SendInviteCompanyEmailRequest : IValidatableMarker
{
    public Guid CompanyId { get; set; }
}
