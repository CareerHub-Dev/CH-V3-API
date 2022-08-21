using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.DTO.Account.Validators;

public class VerifyAdminWithContinuedRegistrationRequestValidator : AbstractValidator<VerifyAdminWithContinuedRegistrationRequest>
{
    public VerifyAdminWithContinuedRegistrationRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
