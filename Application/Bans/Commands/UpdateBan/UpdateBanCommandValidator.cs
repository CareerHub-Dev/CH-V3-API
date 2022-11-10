using FluentValidation;

namespace Application.Bans.Commands.UpdateTag;

public class UpdateBanCommandValidator : AbstractValidator<UpdateBanCommand>
{
    public UpdateBanCommandValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(512);

        RuleFor(x => x.Expires)
            .GreaterThan(x => DateTime.UtcNow);
    }
}
