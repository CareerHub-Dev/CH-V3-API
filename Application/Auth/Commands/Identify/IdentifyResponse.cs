namespace Application.Auth.Commands.Identify;

public class IdentifyResponse
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
}
