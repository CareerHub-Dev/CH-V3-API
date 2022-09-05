using FluentValidation;

namespace Application.Students.Queries;

public class GetStudentsWithPaginationWithSearthWithFilterQueryValidator : AbstractValidator<GetStudentsWithPaginationWithSearthWithFilterQuery>
{
    public GetStudentsWithPaginationWithSearthWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
