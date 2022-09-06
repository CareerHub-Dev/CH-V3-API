namespace Application.CompanyLinks.Queries;

public class CompanyLinkDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }
}
