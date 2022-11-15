using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfCompanyWithPaging;

public class GetStudentSubscribersOfCompanyWithPagingQueryValidator
    : AbstractValidator<GetStudentSubscribersOfCompanyWithPagingQuery>
{
    public GetStudentSubscribersOfCompanyWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
