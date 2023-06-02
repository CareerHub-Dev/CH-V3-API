using FluentValidation;

namespace Application.JobOffers.Queries.GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;

public class GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery>
{
    public GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
