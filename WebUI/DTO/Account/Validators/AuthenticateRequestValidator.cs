using FluentValidation;

namespace WebUI.DTO.Account.Validators;

public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
