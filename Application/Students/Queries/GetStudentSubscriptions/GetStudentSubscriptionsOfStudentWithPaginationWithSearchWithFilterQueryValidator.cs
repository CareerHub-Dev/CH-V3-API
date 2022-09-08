using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscriptions;

public class GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryValidator
    : AbstractValidator<GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery>
{
    public GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
