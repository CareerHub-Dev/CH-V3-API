using FluentValidation;

namespace Application.Common.Entensions;

public static class FluentValidationExtensions
{
    private const string PasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9#?!@$%^&*-]).{8,32}$";
    public const string NureEmailPattern = @"^[A-Za-z]+\.\w+@nure\.ua$";

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(PasswordPattern)
            .WithMessage("The field '{PropertyName}' must match the regular expression '" + PasswordPattern + "'.");
    }

    public static IRuleBuilderOptions<T, string> NureEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(NureEmailPattern)
            .WithMessage("The field '{PropertyName}' must match the regular expression '" +NureEmailPattern + "'.");
    }
}
