namespace WebUI.Common.Models.Company;

public class SendInviteCompanyEmailRequest : IValidatableMarker
{
    public Guid CompanyId { get; set; }
}
