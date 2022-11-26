using Domain.Enums;

namespace Application.Common.DTO.Educations;

public class EducationDTO
{
    public string University { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public Degree Degree { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
