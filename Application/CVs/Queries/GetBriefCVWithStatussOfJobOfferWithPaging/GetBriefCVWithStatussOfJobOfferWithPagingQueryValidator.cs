using FluentValidation;

namespace Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;

public class GetBriefCVWithStatussOfJobOfferWithPagingQueryValidator 
    : AbstractValidator<GetBriefCVWithStatussOfJobOfferWithPagingQuery>
{
	public GetBriefCVWithStatussOfJobOfferWithPagingQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
