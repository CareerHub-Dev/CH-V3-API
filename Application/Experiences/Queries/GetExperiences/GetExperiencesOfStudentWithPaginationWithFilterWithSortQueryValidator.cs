using FluentValidation;

namespace Application.Experiences.Queries.GetExperiences;

public class GetExperiencesOfStudentWithPaginationWithFilterWithSortQueryValidator : AbstractValidator<GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery>
{
    public GetExperiencesOfStudentWithPaginationWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
