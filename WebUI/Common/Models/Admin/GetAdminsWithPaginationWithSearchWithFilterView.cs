namespace WebUI.Common.Models.Admin;

public class GetAdminsWithPaginationWithSearchWithFilterView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public bool? IsSuperAdmin { get; init; }
}
