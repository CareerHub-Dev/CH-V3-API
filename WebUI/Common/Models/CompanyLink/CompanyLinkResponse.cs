using Application.CompanyLinks.Query.Models;

namespace WebUI.Common.Models.CompanyLink;

public class CompanyLinkResponse : CompanyLinkDetailedResponse
{
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public CompanyLinkResponse()
    {

    }

    public CompanyLinkResponse(CompanyLinkDTO model) : base(model)
    {
    }
}
