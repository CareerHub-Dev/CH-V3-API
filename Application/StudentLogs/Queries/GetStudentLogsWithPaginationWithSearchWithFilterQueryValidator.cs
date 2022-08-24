using FluentValidation;

namespace Application.StudentLogs.Queries;

public class GetStudentLogsWithPaginationWithSearchWithFilterQueryValidator : AbstractValidator<GetStudentLogsWithPaginationWithSearchWithFilterQuery>
{
    public GetStudentLogsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
