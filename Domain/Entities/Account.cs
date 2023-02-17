using Domain.Common;

namespace Domain.Entities;

public class Account : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; protected set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string? VerificationToken { get; set; }
    public DateTime? Verified { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public List<Ban> Bans { get; set; } = new List<Ban>();
    public List<Post> Posts { get; set; } = new List<Post>();

    public Guid? PlayerId { get; set; }


    public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
    public bool IsResetTokenExpired => !ResetTokenExpires.HasValue || ResetTokenExpires < DateTime.UtcNow;
}
