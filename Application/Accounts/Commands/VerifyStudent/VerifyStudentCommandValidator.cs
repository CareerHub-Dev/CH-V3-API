using FluentValidation;

namespace Application.Accounts.Commands.VerifyStudent;

public class VerifyStudentCommandValidator : AbstractValidator<VerifyStudentCommand>
{
    public VerifyStudentCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
