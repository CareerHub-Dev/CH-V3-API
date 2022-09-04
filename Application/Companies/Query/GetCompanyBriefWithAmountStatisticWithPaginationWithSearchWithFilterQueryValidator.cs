using FluentValidation;

namespace Application.Companies.Query;

public class GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQueryValidator : AbstractValidator<GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQuery>
{
    public GetCompanyBriefWithAmountStatisticWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
