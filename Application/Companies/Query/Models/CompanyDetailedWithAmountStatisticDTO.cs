namespace Application.Companies.Query.Models;

public class CompanyDetailedWithAmountStatisticDTO : CompanyDetailedDTO
{
    public AmountStatistic AmountStatistic { get; set; } = new AmountStatistic();
}
