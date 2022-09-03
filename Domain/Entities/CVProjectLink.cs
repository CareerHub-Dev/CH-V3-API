using Domain.Common;

namespace Domain.Entities;

public class CVProjectLink : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public Guid CVId { get; set; }
    public CV? CV { get; set; }
}
