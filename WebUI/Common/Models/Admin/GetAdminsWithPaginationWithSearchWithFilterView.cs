namespace WebUI.Common.Models.Admin;

public record GetAdminsWithPaginationWithSearchWithFilterView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsAdminMustBeVerified { get; init; }
    public bool? IsSuperAdmin { get; init; }
}
