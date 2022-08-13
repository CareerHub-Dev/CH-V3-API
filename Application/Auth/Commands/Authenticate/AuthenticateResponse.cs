namespace Application.Auth.Commands.Authenticate;

public class AuthenticateResponse
{
    public Guid AccountId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public DateTime JwtTokenExpires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
