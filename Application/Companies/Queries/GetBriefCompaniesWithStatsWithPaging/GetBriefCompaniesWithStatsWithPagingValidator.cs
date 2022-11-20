using FluentValidation;

namespace Application.Companies.Queries.GetBriefCompaniesWithStatsWithPaginig;

public class GetBriefCompaniesWithStatsWithPagingValidator
    : AbstractValidator<GetBriefCompaniesWithStatsWithPagingQuery>
{
    public GetBriefCompaniesWithStatsWithPagingValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
