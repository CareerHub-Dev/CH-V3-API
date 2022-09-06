using FluentValidation;

namespace Application.Accounts.Query.Identify;

public class IdentifyQueryValidator : AbstractValidator<IdentifyQuery>
{
    public IdentifyQueryValidator()
    {
        RuleFor(x => x.JwtToken)
            .NotEmpty();
    }
}
