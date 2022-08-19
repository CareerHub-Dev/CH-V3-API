namespace WebUI.DTO.Admin;

public class InviteAdminRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
