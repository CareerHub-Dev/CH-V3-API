using Domain.Enums;

namespace Application.Common.DTO.CVs;

public class BriefCVDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ExperienceLevel ExperienceLevel { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
