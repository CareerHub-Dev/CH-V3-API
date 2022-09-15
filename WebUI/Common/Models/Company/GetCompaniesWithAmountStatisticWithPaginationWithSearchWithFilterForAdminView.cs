namespace WebUI.Common.Models.Company;

public record GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterForAdminView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
}
