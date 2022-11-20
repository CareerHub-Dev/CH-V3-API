using Application.Common.Entensions;
using FluentValidation;

namespace Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator 
    : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleForEach(x => x.Images)
            .AllowedExtensions(".jpg", ".jpeg", ".png")
            .MaxFileSize(2 * 1024 * 1024);

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(512);
    }
}
