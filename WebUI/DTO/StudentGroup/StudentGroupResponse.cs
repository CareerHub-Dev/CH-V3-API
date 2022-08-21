using Application.StudentGroups.Queries.Models;

namespace WebUI.DTO.StudentGroup;

public class StudentGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public StudentGroupResponse(StudentGroupDTO studentGroup)
    {
        Id = studentGroup.Id;
        Name = studentGroup.Name;
    }
}
