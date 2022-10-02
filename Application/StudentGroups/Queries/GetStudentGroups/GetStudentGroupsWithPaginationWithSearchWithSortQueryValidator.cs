using FluentValidation;

namespace Application.StudentGroups.Queries.GetStudentGroups;

public class GetStudentGroupsWithPaginationWithSearchWithSortQueryValidator : AbstractValidator<GetStudentGroupsWithPaginationWithSearchWithSortQuery>
{
    public GetStudentGroupsWithPaginationWithSearchWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
