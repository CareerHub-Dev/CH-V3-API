using Domain.Common;

namespace Domain.Entities;

public class Ban : BaseEntity
{
    public string Reason { get; set; } = string.Empty;
    public DateTime Expires { get; set; }

    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
}