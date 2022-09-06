namespace Application.Accounts.Queries.GetAccountBrief;

public class AccountBriefDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public string Role { get; set; } = string.Empty;
}
