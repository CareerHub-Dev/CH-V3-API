using FluentValidation;

namespace Application.Posts.Queries.GetAdmininstrationPostsWithPaging;

public class GetAdmininstrationPostsWithPagingQueryValidator
    : AbstractValidator<GetAdmininstrationPostsWithPagingQuery>
{
    public GetAdmininstrationPostsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
