using FluentValidation;

namespace Application.Accounts.Queries.Authenticate;

public class AuthenticateQueryValidator : AbstractValidator<AuthenticateQuery>
{
    public AuthenticateQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.IpAddress)
            .NotEmpty();
    }
}
