using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;

public class GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
