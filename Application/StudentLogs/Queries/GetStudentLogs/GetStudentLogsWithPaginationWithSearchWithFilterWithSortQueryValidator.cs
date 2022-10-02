using FluentValidation;

namespace Application.StudentLogs.Queries.GetStudentLogs;

public class GetStudentLogsWithPaginationWithSearchWithFilterWithSortQueryValidator : AbstractValidator<GetStudentLogsWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetStudentLogsWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
