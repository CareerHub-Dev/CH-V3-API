using Application.Common.DTO.StudentGroups;

namespace Application.Common.DTO.Students;

public class ShortStudentDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public BriefStudentGroupDTO? StudentGroup { get; set; }
}
