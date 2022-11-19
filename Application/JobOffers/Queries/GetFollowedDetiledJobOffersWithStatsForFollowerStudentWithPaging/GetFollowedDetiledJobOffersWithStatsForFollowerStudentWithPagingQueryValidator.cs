using FluentValidation;

namespace Application.JobOffers.Queries.GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;

public class GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery>
{
    public GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
