using Application.Common.Entensions;
using FluentValidation;

namespace Application.Students.Commands.UpdateStudentPhoto;

public class UpdateStudentPhotoCommandValidator : AbstractValidator<UpdateStudentPhotoCommand>
{
    public UpdateStudentPhotoCommandValidator()
    {
        When(x => x.Photo != null, () =>
        {
            RuleFor(x => x.Photo!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
