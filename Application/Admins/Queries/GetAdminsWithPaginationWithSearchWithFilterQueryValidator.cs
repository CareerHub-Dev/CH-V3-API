using FluentValidation;

namespace Application.Admins.Queries;

public class GetAdminsWithPaginationWithSearchWithFilterQueryValidator : AbstractValidator<GetAdminsWithPaginationWithSearchWithFilterQuery>
{
    public GetAdminsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
