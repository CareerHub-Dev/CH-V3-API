using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudentWithPaging;

public class GetStudentSubscriptionsOfStudentWithPagingQueryValidator
    : AbstractValidator<GetStudentSubscriptionsOfStudentWithPagingQuery>
{
    public GetStudentSubscriptionsOfStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
