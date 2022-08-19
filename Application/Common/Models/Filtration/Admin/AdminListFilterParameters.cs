namespace Application.Common.Models.Filtration.Admin;

public record AdminListFilterParameters : AdminFilterParameters
{
    public Guid? WithoutAdminId { get; init; }
}
