using FluentValidation;

namespace Application.Accounts.Queries.AccountOwnsToken;

public class AccountOwnsTokenQueryValidator : AbstractValidator<AccountOwnsTokenQuery>
{
    public AccountOwnsTokenQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
