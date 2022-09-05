namespace WebUI.Common.Models.Company;

public class GetCompanyBriefWithAmountStatisticsWithVerifyInfoWithPaginationWithSearchWithFilterView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
}
