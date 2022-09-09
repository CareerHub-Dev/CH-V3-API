namespace WebUI.Common.Models.Student;

public record UpdateOwnStudentAccountView
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public DateTime? BirthDate { get; init; }
    public Guid StudentGroupId { get; init; }
}
