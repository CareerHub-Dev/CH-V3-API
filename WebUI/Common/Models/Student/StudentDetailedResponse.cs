using Application.Students.Queries.Models;
using WebUI.Common.Models.StudentGroup;

namespace WebUI.Common.Models.Student;

public class StudentDetailedResponse : StudentBriefResponse
{
    public string? Phone { get; set; }
    public StudentGroupBriefResponse? StudentGroup { get; set; }
    public DateTime? BirthDate { get; set; }

    public StudentDetailedResponse()
    {
            
    }

    public StudentDetailedResponse(StudentDetailedDTO model) : base(model)
    {
        Phone = model.Phone;
        StudentGroup = model.StudentGroup != null ? new StudentGroupBriefResponse(model.StudentGroup) : null;
        BirthDate = model.BirthDate;
    }
}
