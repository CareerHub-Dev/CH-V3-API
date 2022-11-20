using FluentValidation;

namespace Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator 
    : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(512);
    }
}
