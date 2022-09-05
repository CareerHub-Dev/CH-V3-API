namespace Application.Companies.Query.Models;

public class CompanyWithAmountStatisticDTO : CompanyDTO
{
    public AmountStatistic AmountStatistic { get; set; } = new AmountStatistic();
}
