using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentSubscribersOfStudentForFollowerStudent;

public class GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
