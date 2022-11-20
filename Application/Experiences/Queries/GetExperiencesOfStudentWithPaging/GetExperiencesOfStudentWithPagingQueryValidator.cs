using FluentValidation;

namespace Application.Experiences.Queries.GetExperiencesOfStudentWithPaging;

public class GetExperiencesOfStudentWithPagingQueryValidator
    : AbstractValidator<GetExperiencesOfStudentWithPagingQuery>
{
    public GetExperiencesOfStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
