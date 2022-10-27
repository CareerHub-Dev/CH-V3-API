using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfStudent;

public class GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentSubscribersOfStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
