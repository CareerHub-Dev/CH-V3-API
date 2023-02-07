using FluentValidation;

namespace Application.JobOfferReviews.Queries.GetJobOfferReviewsOfStudentWithPaging;

public class GetJobOfferReviewsOfStudentWithPagingQueryValidator
    : AbstractValidator<GetJobOfferReviewsOfStudentWithPagingQuery>
{
    public GetJobOfferReviewsOfStudentWithPagingQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
