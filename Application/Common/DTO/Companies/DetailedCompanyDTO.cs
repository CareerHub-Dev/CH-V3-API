using Application.Common.DTO.CompanyLinks;

namespace Application.Common.DTO.Companies;

public class DetailedCompanyDTO : ShortCompanyDTO
{
    public string Description { get; set; } = string.Empty;
    public List<CompanyLinkDTO> Links { get; set; } = new List<CompanyLinkDTO>();
}
