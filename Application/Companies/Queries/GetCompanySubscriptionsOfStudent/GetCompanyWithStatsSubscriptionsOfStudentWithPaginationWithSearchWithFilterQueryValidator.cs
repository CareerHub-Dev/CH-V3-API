using FluentValidation;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent;

public class GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryValidator
    : AbstractValidator<GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery>
{
    public GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
