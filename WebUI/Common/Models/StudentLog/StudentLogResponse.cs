using Application.StudentLogs.Queries.Models;
using WebUI.Common.Models.StudentGroup;

namespace WebUI.Common.Models.StudentLog;

public class StudentLogResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public StudentGroupBriefResponse? StudentGroup { get; set; }

    public StudentLogResponse(StudentLogDTO studentLog)
    {
        Id = studentLog.Id;
        FirstName = studentLog.FirstName;
        LastName = studentLog.LastName;
        Email = studentLog.Email;
        Created = studentLog.Created;
        CreatedBy = studentLog.CreatedBy;
        LastModified = studentLog.LastModified;
        LastModifiedBy = studentLog.LastModifiedBy;
        StudentGroup = studentLog.StudentGroup != null ? new StudentGroupBriefResponse(studentLog.StudentGroup) : null;
    }
}
