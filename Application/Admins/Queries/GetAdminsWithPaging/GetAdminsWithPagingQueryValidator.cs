using FluentValidation;

namespace Application.Admins.Queries.GetAdminsWithPaging;

public class GetAdminsWithPagingQueryValidator
    : AbstractValidator<GetAdminsWithPagingQuery>
{
    public GetAdminsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
