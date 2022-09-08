﻿using FluentValidation;

namespace Application.Companies.Queries
{
    public class GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQueryValidator 
        : AbstractValidator<GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQuery>
    {
        public GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}