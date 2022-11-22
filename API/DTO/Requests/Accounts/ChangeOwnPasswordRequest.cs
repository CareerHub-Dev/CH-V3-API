namespace API.DTO.Requests.Accounts;

public record ChangeOwnPasswordRequest
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
