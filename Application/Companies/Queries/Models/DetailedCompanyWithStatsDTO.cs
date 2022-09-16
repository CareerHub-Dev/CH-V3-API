namespace Application.Companies.Queries.Models;

public class DetailedCompanyWithStatsDTO : DetailedCompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
