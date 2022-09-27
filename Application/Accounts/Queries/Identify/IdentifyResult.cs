using Domain.Enums;

namespace Application.Accounts.Queries.Identify;

public class IdentifyResult
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public ActivationStatus ActivationStatus { get; set; }
}
