namespace Application.Admins.Queries;

public class AdminDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
    public bool IsSuperAdmin { get; set; }
}
