using Application.Common.DTO.Companies;

namespace Application.Common.DTO.JobOffers;

public class DetiledJobOfferWithStatsWithBriefCompanyDTO : DetiledJobOfferWithStatsDTO
{
    public BriefCompanyDTO Company { get; set; } = new BriefCompanyDTO();
}
