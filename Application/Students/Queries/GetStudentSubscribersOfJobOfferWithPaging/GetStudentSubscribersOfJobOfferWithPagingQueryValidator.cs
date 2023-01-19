using FluentValidation;

namespace Application.Students.Queries.GetStudentSubscribersOfJobOfferWithPaging;

public class GetStudentSubscribersOfJobOfferWithPagingQueryValidator
    : AbstractValidator<GetStudentSubscribersOfJobOfferWithPagingQuery>
{
    public GetStudentSubscribersOfJobOfferWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
