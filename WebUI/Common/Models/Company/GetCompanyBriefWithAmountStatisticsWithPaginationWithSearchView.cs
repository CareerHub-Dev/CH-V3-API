﻿namespace WebUI.Common.Models.Company;

public class GetCompanyBriefWithAmountStatisticsWithPaginationWithSearchView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }
}
