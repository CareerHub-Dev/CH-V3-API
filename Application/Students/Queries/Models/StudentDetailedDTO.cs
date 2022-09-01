using Application.Common.Models.StudentGroup;

namespace Application.Students.Queries.Models;

public class StudentDetailedDTO : StudentBriefDTO
{
    public string? Phone { get; set; }
    public StudentGroupBriefDTO? StudentGroup { get; set; }
    public DateTime? BirthDate { get; set; }
}
