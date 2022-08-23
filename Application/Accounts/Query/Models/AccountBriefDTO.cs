namespace Application.Accounts.Query.Models;

public class AccountBriefDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
}
