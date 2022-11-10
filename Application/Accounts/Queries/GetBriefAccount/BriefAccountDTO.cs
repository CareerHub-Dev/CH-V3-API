using Application.Common.Enums;

namespace Application.Accounts.Queries.GetBriefAccount;

public class BriefAccountDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public Role Role { get; set; }
    public bool IsBanned { get; set; }
}
