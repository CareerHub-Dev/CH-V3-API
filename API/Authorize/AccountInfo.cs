namespace API.Authorize;

public class AccountInfo
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsBanned { get; set; }
}
