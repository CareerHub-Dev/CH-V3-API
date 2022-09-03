using Application.Common.Models.StudentGroup;

namespace WebUI.Common.Models.StudentGroup;

public class StudentGroupBriefResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public StudentGroupBriefResponse()
    {

    }

    public StudentGroupBriefResponse(StudentGroupBriefDTO model)
    {
        Id = model.Id;
        Name = model.Name;
    }
}
