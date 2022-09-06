namespace Application.Companies.Queries.Models;

public class CompanyWithAmountStatisticDTO : CompanyDTO
{
    public AmountStatistic AmountStatistic { get; set; } = new AmountStatistic();
}
