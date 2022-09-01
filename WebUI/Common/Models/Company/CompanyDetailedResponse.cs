using Application.Companies.Query.Models;

namespace WebUI.Common.Models.Company;

public class CompanyDetailedResponse : CompanyBriefResponse
{
    public Guid? BannerId { get; set; }
    public string Motto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public CompanyDetailedResponse()
    {

    }

    public CompanyDetailedResponse(CompanyDetailedDTO model) : base(model)
    {
        BannerId = model.BannerId;
        Motto = model.Motto;
        Description = model.Description;
    }
}
