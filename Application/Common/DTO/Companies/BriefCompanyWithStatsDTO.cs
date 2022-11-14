namespace Application.Common.DTO.Companies;

public class BriefCompanyWithStatsDTO : BriefCompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
