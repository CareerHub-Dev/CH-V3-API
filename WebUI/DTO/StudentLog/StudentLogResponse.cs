using Application.StudentLogs.Queries.Models;
using WebUI.DTO.StudentGroup;

namespace WebUI.DTO.StudentLog;

public class StudentLogResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;

    public StudentGroupResponse? StudentGroup { get; set; }

    public StudentLogResponse(StudentLogDTO studentLog)
    {
        Id = studentLog.Id;
        FirstName = studentLog.FirstName;
        LastName = studentLog.LastName;
        Email = studentLog.Email;
        StudentGroup = studentLog.StudentGroup != null ? new StudentGroupResponse(studentLog.StudentGroup) : null;
    }
}
