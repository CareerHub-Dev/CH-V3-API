using FluentValidation;

namespace Application.Companies.Queries.GetFollowedShortCompaniesWithStatsForFollowerStudentWithPaginig;

public class GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQuery>
{
    public GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
