using FluentValidation;

namespace Application.Posts.Queries.GetPostsOfAccountWithPaging;

public class GetPostsOfAccountWithPagingQueryValidator
    : AbstractValidator<GetPostsOfAccountWithPagingQuery>
{
    public GetPostsOfAccountWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
