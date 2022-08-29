using Application.Common.Models.Image;
using FluentValidation;

namespace Application.Students.Commands.UpdateStudentPhoto;

public class UpdateStudentPhotoCommandValidator : AbstractValidator<UpdateStudentPhotoCommand>
{
    public UpdateStudentPhotoCommandValidator(IValidator<CreateImage> imageValidator)
    {
        When(x => x.Photo != null, () =>
        {
            RuleFor(x => x.Photo)
                .SetValidator(imageValidator!);
        });
    }
}
