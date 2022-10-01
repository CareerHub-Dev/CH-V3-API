using FluentValidation;

namespace Application.Students.Queries.GetStudents;

public class GetStudentsWithPaginationWithSearchWithFilterWithSortQueryValidator : AbstractValidator<GetStudentsWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentsWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
