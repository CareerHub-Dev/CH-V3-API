namespace Application.Accounts.Query.Identify;

public class IdentifyResult
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
}
