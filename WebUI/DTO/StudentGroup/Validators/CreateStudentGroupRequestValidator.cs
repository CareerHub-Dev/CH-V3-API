using FluentValidation;

namespace WebUI.DTO.StudentGroup.Validators;

public class CreateStudentGroupRequestValidator : AbstractValidator<CreateStudentGroupRequest>
{
    public CreateStudentGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20);
    }
}
