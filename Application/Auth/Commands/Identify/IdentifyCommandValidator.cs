using FluentValidation;

namespace Application.Auth.Commands.Identify;

public class IdentifyCommandValidator : AbstractValidator<IdentifyCommand>
{
    public IdentifyCommandValidator()
    {
        RuleFor(x => x.JwtToken)
            .NotEmpty();
    }
}
