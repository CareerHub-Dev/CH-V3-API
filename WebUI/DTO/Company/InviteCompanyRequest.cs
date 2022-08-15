namespace WebUI.DTO.Company;

public class InviteCompanyRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
