using Application.StudentGroups.Queries.Models;

namespace Application.StudentLogs.Queries.Models;

public class StudentLogDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;

    public StudentGroupDTO? StudentGroup { get; set; }
}
