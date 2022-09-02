namespace Application.CompanyLinks.Query.Models;

public class CompanyLinkDetailedDTO : CompanyLinkBriefDTO
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
