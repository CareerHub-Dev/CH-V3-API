namespace Application.Common.DTO.Companies;

public class ShortCompanyWithStatsDTO : ShortCompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
