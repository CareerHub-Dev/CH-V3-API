using Domain.Common;

namespace Domain.Entities;

public class Account : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string? VerificationToken { get; set; }
    public DateTime? Verified { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
}
