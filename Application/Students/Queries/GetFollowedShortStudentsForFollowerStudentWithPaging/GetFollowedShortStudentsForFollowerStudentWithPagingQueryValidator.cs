using FluentValidation;

namespace Application.Students.Queries.GetFollowedShortStudentsForFollowerStudentWithPaging;

public class GetFollowedShortStudentsForFollowerStudentWithPagingQueryValidator
    : AbstractValidator<GetFollowedShortStudentsForFollowerStudentWithPagingQuery>
{
    public GetFollowedShortStudentsForFollowerStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
