using FluentValidation;

namespace Application.RefreshTokens.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandValidator
    : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
