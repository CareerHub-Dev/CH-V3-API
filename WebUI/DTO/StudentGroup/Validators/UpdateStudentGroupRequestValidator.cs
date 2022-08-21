using FluentValidation;

namespace WebUI.DTO.StudentGroup.Validators;

public class UpdateStudentGroupRequestValidator : AbstractValidator<UpdateStudentGroupRequest>
{
    public UpdateStudentGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20);
    }
}
