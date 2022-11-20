using FluentValidation;

namespace Application.JobPositions.Queries.GetJobPositions;

public class GetJobPositionsWithPagingQueryValidator : AbstractValidator<GetJobPositionsWithPagingQuery>
{
    public GetJobPositionsWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
