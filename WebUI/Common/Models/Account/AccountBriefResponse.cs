using Application.Accounts.Query.Models;

namespace WebUI.Common.Models.Account;

public class AccountBriefResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public string Role { get; set; } = string.Empty;

    public AccountBriefResponse()
    {

    }

    public AccountBriefResponse(AccountBriefDTO accountBrief)
    {
        Id = accountBrief.Id;
        Email = accountBrief.Email;
        Verified = accountBrief.Verified;
        PasswordReset = accountBrief.PasswordReset;
        Role = accountBrief.Role;
    }
}
