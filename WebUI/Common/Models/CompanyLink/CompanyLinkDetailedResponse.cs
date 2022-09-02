using Application.CompanyLinks.Query.Models;

namespace WebUI.Common.Models.CompanyLink;

public class CompanyLinkDetailedResponse : CompanyLinkBriefResponse
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }

    public CompanyLinkDetailedResponse()
    {

    }

    public CompanyLinkDetailedResponse(CompanyLinkDetailedDTO model) : base(model)
    {
        Created = model.Created;
        LastModified = model.LastModified;
    }
}
