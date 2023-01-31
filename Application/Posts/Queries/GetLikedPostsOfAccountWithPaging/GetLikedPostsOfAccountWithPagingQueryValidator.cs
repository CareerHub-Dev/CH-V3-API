using FluentValidation;

namespace Application.Posts.Queries.GetLikedPostsOfAccountWithPaging;

public class GetLikedPostsOfAccountWithPagingQueryValidator
    : AbstractValidator<GetLikedPostsOfAccountWithPagingQuery>
{
    public GetLikedPostsOfAccountWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
