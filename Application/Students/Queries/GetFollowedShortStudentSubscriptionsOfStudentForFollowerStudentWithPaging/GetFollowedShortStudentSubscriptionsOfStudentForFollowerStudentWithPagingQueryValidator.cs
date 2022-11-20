using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPaging;

public class GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPagingQuery>
{
    public GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
