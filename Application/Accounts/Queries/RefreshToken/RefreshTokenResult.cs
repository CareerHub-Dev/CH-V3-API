namespace Application.Accounts.Queries.RefreshToken;

public class RefreshTokenResult
{
    public Guid AccountId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public DateTime JwtTokenExpires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpires { get; set; }
}
