namespace Application.Companies.Queries.Models;

public class CompanyDetailedWithAmountStatisticDTO : CompanyDetailedDTO
{
    public AmountStatistic AmountStatistic { get; set; } = new AmountStatistic();
}
