using FluentValidation;

namespace Application.Common.Models.Image;

public class CreateImageValidator : AbstractValidator<CreateImage>
{
    private const int _maxFileSize = 2 * 1024 * 1024;
    private readonly string[] _allowedContentTypes = { "image/jpg", "image/jpeg", "image/png" };
    public CreateImageValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty();

        RuleFor(x => x.Content.Length)
            .LessThanOrEqualTo(_maxFileSize)
            .WithMessage($"Maximum allowed size is {_maxFileSize} bytes.")
            .OverridePropertyName("Content");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(x => _allowedContentTypes.Contains(x)).WithMessage("The field '{PropertyName}' must be in range " + $"[{string.Join(", ", _allowedContentTypes)}]");
    }
}
