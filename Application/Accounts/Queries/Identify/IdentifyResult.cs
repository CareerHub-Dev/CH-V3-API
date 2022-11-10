using Application.Common.Enums;

namespace Application.Accounts.Queries.Identify;

public class IdentifyResult
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public bool IsVerified { get; set; }
    public bool IsBanned { get; set; }
}
