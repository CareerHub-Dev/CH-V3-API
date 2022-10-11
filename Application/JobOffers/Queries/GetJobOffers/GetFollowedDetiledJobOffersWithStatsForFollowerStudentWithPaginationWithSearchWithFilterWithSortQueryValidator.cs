using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOffers;

public class GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
	public GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
