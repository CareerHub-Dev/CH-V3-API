namespace Application.Companies.Queries.Models;

public class CompanyWithStatsDTO : CompanyDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountJobOffers { get; set; }
}
