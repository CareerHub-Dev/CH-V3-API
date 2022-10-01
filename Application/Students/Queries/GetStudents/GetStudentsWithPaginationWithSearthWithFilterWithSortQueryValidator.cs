using FluentValidation;

namespace Application.Students.Queries.GetStudents;

public class GetStudentsWithPaginationWithSearthWithFilterWithSortQueryValidator : AbstractValidator<GetStudentsWithPaginationWithSearthWithFilterWithSortQuery>
{
    public GetStudentsWithPaginationWithSearthWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
