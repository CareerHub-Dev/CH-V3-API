using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfCompany;

public class GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
