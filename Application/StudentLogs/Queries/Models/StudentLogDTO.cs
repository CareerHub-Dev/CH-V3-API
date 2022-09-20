using Application.Common.DTO.StudentGroups;

namespace Application.StudentLogs.Queries.Models;

public class StudentLogDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;

    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public BriefStudentGroupDTO? StudentGroup { get; set; }
}
