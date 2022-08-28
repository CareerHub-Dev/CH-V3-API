using FluentValidation;

namespace WebUI.Common.Models.Account.Validators;

public class VerifyStudentRequestValidator : AbstractValidator<VerifyStudentRequest>
{
    public VerifyStudentRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
