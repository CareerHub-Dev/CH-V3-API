using FluentValidation;

namespace WebUI.Common.Models.StudentGroup.Validators;

public class CreateStudentGroupRequestValidator : AbstractValidator<CreateStudentGroupRequest>
{
    public CreateStudentGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20);
    }
}
