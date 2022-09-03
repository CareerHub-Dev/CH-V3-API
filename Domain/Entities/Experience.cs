using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Experience : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public JobType JobType { get; set; }
    public WorkFormat WorkFormat { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public string JobLocation { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid CVId { get; set; }
    public CV? CV { get; set; }
}
