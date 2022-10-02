using Application.Common.DTO.StudentGroups;

namespace Application.Common.DTO.Students;

public class DetailedStudentDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public BriefStudentGroupDTO? StudentGroup { get; set; }
}
