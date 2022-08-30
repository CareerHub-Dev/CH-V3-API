namespace WebUI.Common.Models.Student;

public class UpdateStudentPhotoRequest : IValidatableMarker
{
    public IFormFile? PhotoFile { get; set; } 
}
