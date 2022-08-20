using FluentValidation;

namespace WebUI.Extentions;

public static class FluentValidationExtentions
{
    public static IRuleBuilderOptions<T, IFormFile> AllowedExtensions<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, params string[] extensions)
    {
        return ruleBuilder
            .Must(x => extensions.Contains(Path.GetExtension(x.FileName))).WithMessage("'{PropertyName}' extension is not allowed!");
    }

    public static IRuleBuilderOptions<T, IFormFile> MaxFileSize<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, int maxFileSize)
    {
        return ruleBuilder
            .Must(x => x.Length <= maxFileSize).WithMessage($"Maximum allowed size is {maxFileSize} bytes.");
    }
}
