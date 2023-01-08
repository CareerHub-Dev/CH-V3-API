using Application.Common.Entensions;
using FluentValidation;

namespace Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator 
    : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Images.Count)
            .LessThanOrEqualTo(10)
            .OverridePropertyName("Images");

        RuleForEach(x => x.Images)
            .AllowedExtensions(".jpg", ".jpeg", ".png")
            .MaxFileSize(2 * 1024 * 1024);

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(512);
    }
}
