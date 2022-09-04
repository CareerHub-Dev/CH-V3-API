namespace Application.Companies.Query.Models;

public class CompanyBriefWithAmountStatisticDTO : CompanyBriefDTO
{
    public AmountStatistic AmountStatistic { get; set; } = new AmountStatistic();
}
