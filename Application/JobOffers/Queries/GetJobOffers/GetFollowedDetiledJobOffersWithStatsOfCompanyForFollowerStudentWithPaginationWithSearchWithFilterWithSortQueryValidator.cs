﻿using FluentValidation;

namespace Application.JobOffers.Queries.GetJobOffers;

public class GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
	public GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
	{
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}
