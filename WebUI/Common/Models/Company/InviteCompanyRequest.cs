namespace WebUI.Common.Models.Company;

public class InviteCompanyRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
