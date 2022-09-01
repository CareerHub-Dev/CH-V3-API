using Application.Students.Queries.Models;

namespace WebUI.Common.Models.Student;

public class StudentBriefResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; }

    public StudentBriefResponse()
    {

    }

    public StudentBriefResponse(StudentBriefDTO model)
    {
        Id = model.Id;
        FirstName = model.FirstName;
        LastName = model.LastName;
        Email = model.Email;
        PhotoId = model.PhotoId;
    }
}
