namespace Application.Common.DTO.CompanyLinks;

public class CompanyLinkDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}
