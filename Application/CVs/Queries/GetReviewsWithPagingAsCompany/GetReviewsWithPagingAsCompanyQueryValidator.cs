using FluentValidation;

namespace Application.CVs.Queries.GetReviewsWithPagingAsCompany;

public class GetReviewsWithPagingAsCompanyQueryValidator
    : AbstractValidator<GetReviewsWithPagingAsCompanyQuery>
{
    public GetReviewsWithPagingAsCompanyQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
