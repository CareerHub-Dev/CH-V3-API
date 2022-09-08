namespace WebUI.Common.Models.Student;

public record GetStudentsWithPaginationWithSearthWithFilterForAdminView
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
}
