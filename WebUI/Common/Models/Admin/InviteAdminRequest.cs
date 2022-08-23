namespace WebUI.Common.Models.Admin;

public class InviteAdminRequest : IValidatableMarker
{
    public string Email { get; set; } = string.Empty;
}
