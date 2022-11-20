using FluentValidation;

namespace Application.Accounts.Queries.Identify;

public class IdentifyQueryValidator
    : AbstractValidator<IdentifyQuery>
{
    public IdentifyQueryValidator()
    {
        RuleFor(x => x.JwtToken)
            .NotEmpty();
    }
}
