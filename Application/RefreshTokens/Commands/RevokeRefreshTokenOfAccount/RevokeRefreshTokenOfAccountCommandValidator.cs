using FluentValidation;

namespace Application.RefreshTokens.Commands.RevokeRefreshTokenOfAccount;

public class RevokeRefreshTokenOfAccountCommandValidator
    : AbstractValidator<RevokeRefreshTokenOfAccountCommand>
{
    public RevokeRefreshTokenOfAccountCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
