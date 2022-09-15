using FluentValidation;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent
{
    public class GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQueryValidator
        : AbstractValidator<GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQuery>
    {
        public GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
