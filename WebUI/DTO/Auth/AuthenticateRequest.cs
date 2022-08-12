namespace WebUI.DTO.Auth;

public class AuthenticateRequest
{
    public string Email { set; get; } = string.Empty;
    public string Password { set; get; } = string.Empty;
}
