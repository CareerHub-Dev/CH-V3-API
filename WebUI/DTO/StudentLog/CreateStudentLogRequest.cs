namespace WebUI.DTO.StudentLog;

public class CreateStudentLogRequest : IValidatableMarker
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public Guid StudentGroupId { get; set; }
}
