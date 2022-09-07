using FluentValidation;

namespace Application.Students.Queries;

public class GetFollowedStudentDetailedsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQueryValidator
    : AbstractValidator<GetFollowedStudentDetailedsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery>
{
    public GetFollowedStudentDetailedsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
