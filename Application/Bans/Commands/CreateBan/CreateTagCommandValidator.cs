using FluentValidation;

namespace Application.Bans.Commands.CreateBan;

public class CreateBanCommandValidator : AbstractValidator<CreateBanCommand>
{
    public CreateBanCommandValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(512);

        RuleFor(x => x.Expires)
            .GreaterThan(x => DateTime.UtcNow);
    }
}
