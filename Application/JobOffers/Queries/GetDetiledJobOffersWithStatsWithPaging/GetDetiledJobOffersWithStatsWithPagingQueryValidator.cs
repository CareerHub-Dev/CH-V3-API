using FluentValidation;

namespace Application.JobOffers.Queries.GetDetiledJobOffersWithStatsWithPaging;

public class GetDetiledJobOffersWithStatsWithPagingQueryValidator
    : AbstractValidator<GetDetiledJobOffersWithStatsWithPagingQuery>
{
	public GetDetiledJobOffersWithStatsWithPagingQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
