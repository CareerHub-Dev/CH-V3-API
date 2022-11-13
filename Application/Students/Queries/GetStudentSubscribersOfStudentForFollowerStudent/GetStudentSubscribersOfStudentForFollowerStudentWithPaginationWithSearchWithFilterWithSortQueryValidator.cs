using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfStudentForFollowerStudent;

public class GetStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentSubscribersOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
