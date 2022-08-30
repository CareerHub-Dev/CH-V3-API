namespace WebUI.Common.Models.Student;

public class UpdateStudentRequest : IValidatableMarker
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid StudentGroupId { get; set; }
    public DateTime? BirthDate { get; set; }
}
