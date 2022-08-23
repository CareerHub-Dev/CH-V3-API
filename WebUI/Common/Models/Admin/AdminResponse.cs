using Application.Admins.Queries.Models;

namespace WebUI.Common.Models.Admin;

public class AdminResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }

    public AdminResponse()
    {

    }

    public AdminResponse(AdminDTO model)
    {
        Id = model.Id;
        Email = model.Email;
        Verified = model.Verified;
        PasswordReset = model.PasswordReset;
    }
}
