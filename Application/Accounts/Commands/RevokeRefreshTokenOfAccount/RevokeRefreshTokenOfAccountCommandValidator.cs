using FluentValidation;

namespace Application.Accounts.Commands.RevokeRefreshTokenOfAccount;

public class RevokeRefreshTokenOfAccountCommandValidator : AbstractValidator<RevokeRefreshTokenOfAccountCommand>
{
    public RevokeRefreshTokenOfAccountCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
