namespace Application.Companies.Query.Models;

public class CompanyBriefDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? LogoId { get; set; }
    public Guid? BannerId { get; set; }
}
