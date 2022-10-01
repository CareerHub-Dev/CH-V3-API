using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudent;

public class GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
