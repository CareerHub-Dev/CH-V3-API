using Domain.Common;

namespace Domain.Entities;

public class StudentGroup : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public List<Student> Students = new List<Student>();
    public List<StudentLog> StudentLogs = new List<StudentLog>();
}
