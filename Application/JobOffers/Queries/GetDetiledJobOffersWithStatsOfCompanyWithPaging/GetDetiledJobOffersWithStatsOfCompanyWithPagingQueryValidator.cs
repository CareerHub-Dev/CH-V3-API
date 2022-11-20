using FluentValidation;

namespace Application.JobOffers.Queries.GetDetiledJobOffersWithStatsOfCompanyWithPaging;

public class GetDetiledJobOffersWithStatsOfCompanyWithPagingQueryValidator
    : AbstractValidator<GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery>
{
    public GetDetiledJobOffersWithStatsOfCompanyWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
