using FluentValidation;

namespace Application.Students.Queries.GetStudents;

public class GetStudentsWithPagingQueryValidator 
    : AbstractValidator<GetStudentsWithPagingQuery>
{
    public GetStudentsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
