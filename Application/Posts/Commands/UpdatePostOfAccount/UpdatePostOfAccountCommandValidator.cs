using FluentValidation;

namespace Application.Posts.Commands.UpdatePostOfAccount;

public class UpdatePostOfAccountCommandValidator : AbstractValidator<UpdatePostOfAccountCommand>
{
    public UpdatePostOfAccountCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(512);
    }
}
