using Application.CompanyLinks.Query.Models;

namespace WebUI.Common.Models.CompanyLink;

public class CompanyLinkBriefResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public CompanyLinkBriefResponse()
    {

    }

    public CompanyLinkBriefResponse(CompanyLinkBriefDTO model)
    {
        Id = model.Id;
        Name = model.Name;
        Uri = model.Uri;
        CompanyId = model.CompanyId;
    }
}
