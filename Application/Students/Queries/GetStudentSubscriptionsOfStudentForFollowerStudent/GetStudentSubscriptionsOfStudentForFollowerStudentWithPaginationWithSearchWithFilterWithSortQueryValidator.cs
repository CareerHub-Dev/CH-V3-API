using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudentForFollowerStudent;

public class GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
