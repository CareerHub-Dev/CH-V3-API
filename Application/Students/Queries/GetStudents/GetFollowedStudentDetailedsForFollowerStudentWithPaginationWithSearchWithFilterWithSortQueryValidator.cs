using FluentValidation;

namespace Application.Students.Queries.GetStudents;

public class GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
