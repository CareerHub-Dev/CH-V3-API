using FluentValidation;

namespace Application.Accounts.Queries.RefreshToken;

public class RefreshTokenQueryValidator
    : AbstractValidator<RefreshTokenQuery>
{
    public RefreshTokenQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
