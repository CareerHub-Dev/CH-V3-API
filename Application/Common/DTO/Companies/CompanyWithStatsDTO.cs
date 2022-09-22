namespace Application.Common.DTO.Companies;

public class CompanyWithStatsDTO : CompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
