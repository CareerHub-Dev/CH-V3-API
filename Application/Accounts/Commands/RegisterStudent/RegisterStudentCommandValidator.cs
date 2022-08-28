using Application.Common.Entensions;
using FluentValidation;

namespace Application.Accounts.Commands.RegisterStudent;

public class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NureEmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
