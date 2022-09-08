using FluentValidation;

namespace Application.Accounts.Commands.RevokeToken;

public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
