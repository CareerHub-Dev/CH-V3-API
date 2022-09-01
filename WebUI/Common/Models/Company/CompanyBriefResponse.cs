using Application.Companies.Query.Models;

namespace WebUI.Common.Models.Company;

public class CompanyBriefResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? LogoId { get; set; }

    public CompanyBriefResponse()
    {

    }

    public CompanyBriefResponse(CompanyBriefDTO model)
    {
        Id = model.Id;
        Name = model.Name;
        Email = model.Email;
        LogoId = model.LogoId;
    }
}
