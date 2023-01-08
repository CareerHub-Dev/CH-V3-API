using FluentValidation;

namespace Application.Posts.Commands.UpdatePostOfAccount;

public class UpdatePostOfAccountCommandValidator 
    : AbstractValidator<UpdatePostOfAccountCommand>
{
    public UpdatePostOfAccountCommandValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(512).When(x => x.Text != null);
    }
}
