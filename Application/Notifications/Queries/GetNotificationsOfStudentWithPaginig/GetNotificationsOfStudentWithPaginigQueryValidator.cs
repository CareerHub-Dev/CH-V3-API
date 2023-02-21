using FluentValidation;

namespace Application.Notifications.Queries.GetNotificationsOfStudentWithPaginig;

public class GetNotificationsOfStudentWithPaginigQueryValidator
    : AbstractValidator<GetNotificationsOfStudentWithPaginigQuery>
{
    public GetNotificationsOfStudentWithPaginigQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
