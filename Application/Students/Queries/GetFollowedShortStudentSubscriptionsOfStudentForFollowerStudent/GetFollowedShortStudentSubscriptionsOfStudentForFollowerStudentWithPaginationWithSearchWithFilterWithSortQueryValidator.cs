using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudent;

public class GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
