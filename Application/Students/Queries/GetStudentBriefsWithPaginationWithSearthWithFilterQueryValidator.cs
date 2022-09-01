using FluentValidation;

namespace Application.Students.Queries;

public class GetStudentBriefsWithPaginationWithSearthWithFilterQueryValidator : AbstractValidator<GetStudentBriefsWithPaginationWithSearthWithFilterQuery>
{
    public GetStudentBriefsWithPaginationWithSearthWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
