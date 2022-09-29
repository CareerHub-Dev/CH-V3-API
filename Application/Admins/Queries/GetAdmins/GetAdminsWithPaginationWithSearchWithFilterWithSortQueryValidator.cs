using FluentValidation;

namespace Application.Admins.Queries.GetAdmins;

public class GetAdminsWithPaginationWithSearchWithFilterWithSortQueryValidator : AbstractValidator<GetAdminsWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetAdminsWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
