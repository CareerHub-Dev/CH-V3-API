using FluentValidation;

namespace Application.Posts.Queries.GetPostsWithPaging;

internal class GetPostsWithPagingQueryValidator
    : AbstractValidator<GetPostsWithPagingQuery>
{
	public GetPostsWithPagingQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
