using FluentValidation;

namespace Application.Companies.Query;

public class GetCompanyBriefsWithPaginationWithSearchWithFilterQueryValidator 
    : AbstractValidator<GetCompanyBriefsWithPaginationWithSearchWithFilterQuery>
{
    public GetCompanyBriefsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
