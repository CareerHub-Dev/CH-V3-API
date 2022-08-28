namespace WebUI.Common.Models.Account;

public class VerifyStudentRequest : IValidatableMarker
{
    public string Token { get; set; } = string.Empty;
}
