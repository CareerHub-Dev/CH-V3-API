using FluentValidation;

namespace Application.Companies.Queries.GetBriefCompanies;

public class GetBriefCompaniesWithPagingValidator
    : AbstractValidator<GetBriefCompaniesWithPagingQuery>
{
    public GetBriefCompaniesWithPagingValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
