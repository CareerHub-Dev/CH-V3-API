using FluentValidation;

namespace Application.StudentGroups.Queries.GetStudentGroupsWithPaging;

public class GetStudentGroupsWithPagingQueryValidator
    : AbstractValidator<GetStudentGroupsWithPagingQuery>
{
    public GetStudentGroupsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
