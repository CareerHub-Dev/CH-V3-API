using Domain.Common;

namespace Domain.Entities;

public class StudentLog : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public string NormalizedEmail { set; get; } = string.Empty;

    public Guid StudentGroupId { get; set; }
    public StudentGroup? StudentGroup { get; set; }
}
