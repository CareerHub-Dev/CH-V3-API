using FluentValidation;

namespace Application.Companies.Queries.GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPaging;

public class GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingValidator
    : AbstractValidator<GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery>
{
    public GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
