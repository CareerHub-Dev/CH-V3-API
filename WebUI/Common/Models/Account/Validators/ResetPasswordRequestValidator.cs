using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.Account.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
