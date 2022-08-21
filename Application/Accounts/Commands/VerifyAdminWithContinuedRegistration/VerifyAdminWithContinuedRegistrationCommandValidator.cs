using Application.Common.Entensions;
using FluentValidation;

namespace Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;

public class VerifyAdminWithContinuedRegistrationCommandValidator : AbstractValidator<VerifyAdminWithContinuedRegistrationCommand>
{
    public VerifyAdminWithContinuedRegistrationCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
