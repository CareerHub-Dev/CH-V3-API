using FluentValidation;

namespace Application.Tags.Queries.GetTagsWithPaging;

public class GetTagsWithPagingQueryValidator
    : AbstractValidator<GetTagsWithPagingQuery>
{
    public GetTagsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
