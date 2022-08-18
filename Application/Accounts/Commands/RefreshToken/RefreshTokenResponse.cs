namespace Application.Accounts.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public Guid AccountId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public DateTime JwtTokenExpires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
