using FluentValidation;

namespace Application.Accounts.Commands.ChangeActivationStatus;

public class ChangeActivationStatusCommandValidator : AbstractValidator<ChangeActivationStatusCommand>
{
    public ChangeActivationStatusCommandValidator()
    {
        RuleFor(x => x.ActivationStatus)
            .IsInEnum();
    }
}
