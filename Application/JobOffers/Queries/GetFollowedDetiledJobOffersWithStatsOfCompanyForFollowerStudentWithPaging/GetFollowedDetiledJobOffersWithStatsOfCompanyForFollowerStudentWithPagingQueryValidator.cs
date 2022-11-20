using FluentValidation;

namespace Application.JobOffers.Queries.GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaging;

public class GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPagingQuery>
{
    public GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
