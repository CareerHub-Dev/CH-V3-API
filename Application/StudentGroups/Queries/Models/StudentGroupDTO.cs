using Application.Common.Models.StudentGroup;

namespace Application.StudentGroups.Queries.Models;

public class StudentGroupDTO : StudentGroupBriefDTO
{
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
