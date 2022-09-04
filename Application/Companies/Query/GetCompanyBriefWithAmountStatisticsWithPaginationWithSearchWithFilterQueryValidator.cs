using FluentValidation;

namespace Application.Companies.Query;

public class GetCompanyBriefWithAmountStatisticsWithPaginationWithSearchWithFilterQueryValidator : AbstractValidator<GetCompanyBriefWithAmountStatisticsWithPaginationWithSearchWithFilterQuery>
{
    public GetCompanyBriefWithAmountStatisticsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
