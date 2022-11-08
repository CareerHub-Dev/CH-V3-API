using Domain.Enums;

namespace Domain.Entities;

public class Education
{
    public string University { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public Degree Degree { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
}
