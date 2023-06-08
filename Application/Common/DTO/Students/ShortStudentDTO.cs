using Application.Common.DTO.StudentGroups;

namespace Application.Common.DTO.Students;

public class ShortStudentDTO
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string? Photo { get; set; }
    public required BriefStudentGroupDTO? StudentGroup { get; set; }
}
