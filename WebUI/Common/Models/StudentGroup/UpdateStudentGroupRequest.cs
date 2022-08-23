namespace WebUI.Common.Models.StudentGroup;

public class UpdateStudentGroupRequest : IValidatableMarker
{
    public string Name { get; set; } = string.Empty;
}
