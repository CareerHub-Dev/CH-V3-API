using FluentValidation;

namespace Application.Accounts.Commands.Identify;

public class IdentifyCommandValidator : AbstractValidator<IdentifyCommand>
{
    public IdentifyCommandValidator()
    {
        RuleFor(x => x.JwtToken)
            .NotEmpty();
    }
}
