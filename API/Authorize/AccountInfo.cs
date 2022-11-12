using Application.Common.Enums;

namespace API.Authorize;

public class AccountInfo
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public bool IsBanned { get; set; }
}
