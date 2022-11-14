using FluentValidation;

namespace Application.Companies.Queries.GetBriefCompanySubscriptionsWithStatsOfStudentWithPaging;

public class GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingValidator
    : AbstractValidator<GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQuery>
{
    public GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
