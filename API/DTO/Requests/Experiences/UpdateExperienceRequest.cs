using Domain.Enums;

namespace API.DTO.Requests.Experiences;

public record UpdateExperienceRequest
{
    public Guid ExperienceId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public JobType JobType { get; init; }
    public WorkFormat WorkFormat { get; init; }
    public ExperienceLevel ExperienceLevel { get; init; }
    public string JobLocation { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
