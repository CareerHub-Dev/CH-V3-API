using FluentValidation;

namespace Application.Posts.Queries.GetPostsOfFollowedAccountsForStudentWithPaging;

public class GetPostsOfFollowedAccountsForStudentWithPagingQueryValidator
    : AbstractValidator<GetPostsOfFollowedAccountsForStudentWithPagingQuery>
{
    public GetPostsOfFollowedAccountsForStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}