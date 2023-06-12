using FluentValidation;

namespace Application.Notifications.Queries.GetUnviewedNotificationsOfStudentWithPaginig;

public class GetUnviewedNotificationsOfStudentWithPaginigQueryValidator
    : AbstractValidator<GetUnviewedNotificationsOfStudentWithPaginigQuery>
{
    public GetUnviewedNotificationsOfStudentWithPaginigQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
