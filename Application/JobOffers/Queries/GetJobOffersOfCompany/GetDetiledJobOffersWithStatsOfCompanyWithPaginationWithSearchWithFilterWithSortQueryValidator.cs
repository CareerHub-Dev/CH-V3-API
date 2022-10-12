using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOffersOfCompany;

public class GetDetiledJobOffersWithStatsOfCompanyWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetDetiledJobOffersWithStatsOfCompanyWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetDetiledJobOffersWithStatsOfCompanyWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
