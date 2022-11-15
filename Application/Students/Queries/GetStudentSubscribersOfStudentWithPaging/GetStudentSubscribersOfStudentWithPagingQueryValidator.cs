using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfStudentWithPaging;

public class GetStudentSubscribersOfStudentWithPagingQueryValidator
    : AbstractValidator<GetStudentSubscribersOfStudentWithPagingQuery>
{
    public GetStudentSubscribersOfStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
