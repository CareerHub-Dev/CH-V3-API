namespace WebUI.DTO.StudentGroup;

public class CreateStudentGroupRequest : IValidatableMarker
{
    public string Name { get; set; } = string.Empty;
}
