using Application.StudentGroups.Queries.Models;

namespace WebUI.Common.Models.StudentGroup;

public class StudentGroupResponse : StudentGroupBriefResponse
{
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public StudentGroupResponse()
    {

    }

    public StudentGroupResponse(StudentGroupDTO studentGroup) : base(studentGroup)
    {
        Created = studentGroup.Created;
        CreatedBy = studentGroup.CreatedBy;
        LastModified = studentGroup.LastModified;
        LastModifiedBy = studentGroup.LastModifiedBy;
    }
}
