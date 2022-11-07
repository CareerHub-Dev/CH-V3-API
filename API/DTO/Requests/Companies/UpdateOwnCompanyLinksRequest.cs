using Application.Common.DTO.CompanyLinks;

namespace API.DTO.Requests.Companies;

public class UpdateOwnCompanyLinksRequest
{
    public List<CompanyLinkDTO> Links { get; init; } = new List<CompanyLinkDTO>();
}
