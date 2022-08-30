using Domain.Common;

namespace Domain.Entities;

public class CompanyLink : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}
