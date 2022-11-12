using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;

public class GetDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
