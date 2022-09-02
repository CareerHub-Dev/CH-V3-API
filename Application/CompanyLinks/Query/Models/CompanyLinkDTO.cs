namespace Application.CompanyLinks.Query.Models;

public class CompanyLinkDTO : CompanyLinkDetailedDTO
{
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
