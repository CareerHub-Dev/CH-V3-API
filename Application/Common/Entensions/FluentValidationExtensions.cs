﻿using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Entensions;

public static class FluentValidationExtensions
{
    private const string PasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9#?!@$%^&*-]).{8,32}$";
    public const string NureEmailPattern = @"^[A-Za-z]+\.\w+@nure\.ua$";
    public const string PhonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
    public const string UriPattern = @"^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?";

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(PasswordPattern)
            .WithMessage($"The field '{{PropertyName}}' must match the regular expression '{PasswordPattern}'.");
    }

    public static IRuleBuilderOptions<T, string> NureEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(NureEmailPattern)
            .WithMessage($"The field '{{PropertyName}}' must match the regular expression '{NureEmailPattern}'.");
    }

    public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(PhonePattern)
            .WithMessage($"The field '{{PropertyName}}' must match the regular expression '{PhonePattern}'.");
    }

    public static IRuleBuilderOptions<T, string> Uri<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(UriPattern)
            .WithMessage($"The field '{{PropertyName}}' must match the regular expression '{UriPattern}'.");
    }

    public static IRuleBuilderOptions<T, IFormFile> AllowedExtensions<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder,
        params string[] extensions)
    {
        return ruleBuilder
            .Must(x => extensions.Contains(Path.GetExtension(x.FileName)))
            .WithMessage("'{PropertyName}' extension is not allowed!");
    }

    public static IRuleBuilderOptions<T, IFormFile> MaxFileSize<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, int maxFileSize)
    {
        return ruleBuilder
            .Must(x => x.Length <= maxFileSize).WithMessage($"Maximum allowed size is {maxFileSize} bytes.");
    }
}
