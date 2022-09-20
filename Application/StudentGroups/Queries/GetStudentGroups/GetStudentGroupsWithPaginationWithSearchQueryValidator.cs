using FluentValidation;

namespace Application.StudentGroups.Queries.GetStudentGroups;

public class GetStudentGroupsWithPaginationWithSearchQueryValidator : AbstractValidator<GetStudentGroupsWithPaginationWithSearchQuery>
{
    public GetStudentGroupsWithPaginationWithSearchQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
