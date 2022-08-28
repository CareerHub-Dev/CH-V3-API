using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.Account.Validators;

public class RegisterStudentRequestValidator : AbstractValidator<RegisterStudentRequest>
{
    public RegisterStudentRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NureEmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
