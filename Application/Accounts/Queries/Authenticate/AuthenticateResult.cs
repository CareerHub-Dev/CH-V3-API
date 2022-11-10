using Application.Common.Enums;

namespace Application.Accounts.Queries.Authenticate;

public class AuthenticateResult
{
    public Guid AccountId { get; set; }
    public Role Role { get; set; }
    public string JwtToken { get; set; } = string.Empty;
    public DateTime JwtTokenExpires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpires { get; set; }
}
