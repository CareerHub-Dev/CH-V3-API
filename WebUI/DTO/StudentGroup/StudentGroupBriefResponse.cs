using Application.Common.Models.StudentGroup;

namespace WebUI.DTO.StudentGroup;

public class StudentGroupBriefResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public StudentGroupBriefResponse()
    {

    }

    public StudentGroupBriefResponse(StudentGroupBriefDTO studentGroup)
    {
        Id = studentGroup.Id;
        Name = studentGroup.Name;
    }
}
