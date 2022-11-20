using FluentValidation;

namespace Application.Accounts.Queries.Authenticate;

public class AuthenticateQueryValidator
    : AbstractValidator<AuthenticateQuery>
{
    public AuthenticateQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(256)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
