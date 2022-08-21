using FluentValidation;

namespace Application.Accounts.Commands.AccountOwnsToken;

public class AccountOwnsTokenCommandValidator : AbstractValidator<AccountOwnsTokenCommand>
{
    public AccountOwnsTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
