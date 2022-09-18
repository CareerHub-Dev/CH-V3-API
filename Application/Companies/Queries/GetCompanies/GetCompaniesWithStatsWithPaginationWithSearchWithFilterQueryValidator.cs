﻿using FluentValidation;

namespace Application.Companies.Queries.GetCompanies;

public class GetCompaniesWithStatsWithPaginationWithSearchWithFilterQueryValidator
    : AbstractValidator<GetCompaniesWithStatsWithPaginationWithSearchWithFilterQuery>
{
    public GetCompaniesWithStatsWithPaginationWithSearchWithFilterQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(50);
    }
}