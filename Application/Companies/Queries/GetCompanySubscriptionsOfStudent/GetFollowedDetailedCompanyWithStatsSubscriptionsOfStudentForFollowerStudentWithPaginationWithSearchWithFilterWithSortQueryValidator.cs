﻿using FluentValidation;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent;

public class GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator
    : AbstractValidator<GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery>
{
    public GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}