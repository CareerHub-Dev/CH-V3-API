using FluentValidation;

namespace Application.StudentLogs.Queries.GetStudentLogsWithPaging;

public class GetStudentLogsWithPagingQueryValidator
    : AbstractValidator<GetStudentLogsWithPagingQuery>
{
    public GetStudentLogsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
