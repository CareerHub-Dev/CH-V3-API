using FluentValidation;

namespace Application.CVs.Queries.GetBriefCVsOfStudentWithPaging;

public class GetBriefCVsOfStudentWithPagingQueryValidator
    : AbstractValidator<GetBriefCVsOfStudentWithPagingQuery>
{
	public GetBriefCVsOfStudentWithPagingQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
