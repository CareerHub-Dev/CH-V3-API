using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaging;

public class GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPagingQuery>
{
    public GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
