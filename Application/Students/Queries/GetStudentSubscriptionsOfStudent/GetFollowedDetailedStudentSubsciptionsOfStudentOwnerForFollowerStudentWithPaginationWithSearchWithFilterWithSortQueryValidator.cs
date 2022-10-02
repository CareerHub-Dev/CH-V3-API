using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscriptionsOfStudent;

public class GetFollowedDetailedStudentSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetailedStudentSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedDetailedStudentSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
