using FluentValidation;

namespace Application.Accounts.Query;

public class AccountOwnsTokenQueryValidator : AbstractValidator<AccountOwnsTokenQuery>
{
    public AccountOwnsTokenQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
