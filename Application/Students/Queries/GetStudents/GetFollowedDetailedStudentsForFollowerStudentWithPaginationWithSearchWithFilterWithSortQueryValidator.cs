using FluentValidation;

namespace Application.Students.Queries.GetStudents;

public class GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
