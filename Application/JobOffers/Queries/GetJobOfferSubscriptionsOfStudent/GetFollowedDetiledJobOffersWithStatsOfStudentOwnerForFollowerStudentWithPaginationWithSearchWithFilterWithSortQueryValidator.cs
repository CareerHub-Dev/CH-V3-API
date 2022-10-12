using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;

public class GetFollowedDetiledJobOffersWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOffersWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedDetiledJobOffersWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
