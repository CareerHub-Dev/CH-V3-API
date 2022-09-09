using FluentValidation;

namespace Application.Accounts.Queries.AccountOwnsRefreshTokenWithFilter;

public class AccountOwnsRefreshTokenWithFilterValidator : AbstractValidator<AccountOwnsRefreshTokenWithFilterQuery>
{
    public AccountOwnsRefreshTokenWithFilterValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
