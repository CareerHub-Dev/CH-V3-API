using FluentValidation;

namespace Application.Common.Entensions;

public static class FluentValidationExtensions
{
    private const string PasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9#?!@$%^&*-]).{8,32}$";

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(PasswordPattern)
            .WithMessage("The field '{PropertyName}' must match the regular expression '" + PasswordPattern + "'.");
    }
}
