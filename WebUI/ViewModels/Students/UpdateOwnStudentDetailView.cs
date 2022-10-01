namespace WebUI.ViewModels.Students;

public record UpdateOwnStudentDetailView
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public DateTime? BirthDate { get; init; }
    public Guid StudentGroupId { get; init; }
}
