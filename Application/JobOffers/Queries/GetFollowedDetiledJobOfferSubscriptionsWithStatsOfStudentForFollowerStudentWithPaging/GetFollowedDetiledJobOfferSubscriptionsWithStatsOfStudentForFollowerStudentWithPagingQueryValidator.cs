using FluentValidation;

namespace Application.JobOffers.Queries.GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPaging;

public class GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery>
{
    public GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
