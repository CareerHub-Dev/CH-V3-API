using FluentValidation;

namespace Application.JobOffers.Queries.GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPaging;

public class GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPagingQueryValidator
    : AbstractValidator<GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPagingQuery>
{
    public GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
