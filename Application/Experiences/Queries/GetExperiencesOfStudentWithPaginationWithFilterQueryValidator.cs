using FluentValidation;

namespace Application.Experiences.Queries;

public class GetExperiencesOfStudentWithPaginationWithFilterQueryValidator : AbstractValidator<GetExperiencesOfStudentWithPaginationWithFilterQuery>
{
    public GetExperiencesOfStudentWithPaginationWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
