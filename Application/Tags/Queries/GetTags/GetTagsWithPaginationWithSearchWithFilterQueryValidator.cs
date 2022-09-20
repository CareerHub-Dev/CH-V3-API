using FluentValidation;

namespace Application.Tags.Queries.GetTags;

public class GetTagsWithPaginationWithSearchWithFilterQueryValidator : AbstractValidator<GetTagsWithPaginationWithSearchWithFilterQuery>
{
    public GetTagsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
