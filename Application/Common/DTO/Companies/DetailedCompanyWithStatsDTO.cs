namespace Application.Common.DTO.Companies;

public class DetailedCompanyWithStatsDTO : DetailedCompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
