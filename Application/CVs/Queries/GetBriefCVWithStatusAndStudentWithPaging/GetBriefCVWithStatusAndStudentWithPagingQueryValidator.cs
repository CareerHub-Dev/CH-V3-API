using FluentValidation;

namespace Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;

public class GetBriefCVWithStatusAndStudentWithPagingQueryValidator
    : AbstractValidator<GetBriefCVWithStatusAndStudentWithPagingQuery>
{
    public GetBriefCVWithStatusAndStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
