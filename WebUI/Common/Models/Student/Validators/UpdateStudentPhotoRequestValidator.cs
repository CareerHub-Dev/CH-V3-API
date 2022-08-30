using FluentValidation;
using WebUI.Common.Extentions;

namespace WebUI.Common.Models.Student.Validators;

public class UpdateStudentPhotoRequestValidator : AbstractValidator<UpdateStudentPhotoRequest>
{
    public UpdateStudentPhotoRequestValidator()
    {
        When(x => x.PhotoFile != null, () =>
        {
            RuleFor(x => x.PhotoFile!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
