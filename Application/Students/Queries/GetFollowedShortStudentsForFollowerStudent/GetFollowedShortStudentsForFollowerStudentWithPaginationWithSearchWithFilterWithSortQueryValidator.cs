using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentsForFollowerStudent;

public class GetFollowedShortStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedShortStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedShortStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
