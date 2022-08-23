using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.StudentLog.Validators;

public class CreateStudentLogRequestValidator : AbstractValidator<CreateStudentLogRequest>
{
    public CreateStudentLogRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NureEmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(30);
    }
}
