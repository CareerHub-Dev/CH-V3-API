using Domain.Enums;

namespace Application.Accounts.Queries.GetBriefAccount;

public class BriefAccountDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public string Role { get; set; } = string.Empty;
    public ActivationStatus ActivationStatus { get; set; }
}
