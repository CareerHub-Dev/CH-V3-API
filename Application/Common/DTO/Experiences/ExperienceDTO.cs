using Domain.Enums;

namespace Application.Common.DTO.Experiences;

public class ExperienceDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public JobType JobType { get; set; }
    public WorkFormat WorkFormat { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public string JobLocation { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid StudentId { get; set; }
}
