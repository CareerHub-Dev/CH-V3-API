namespace Application.Common.DTO.JobOffers;

public class DetiledJobOfferWithStatsDTO : DetiledJobOfferDTO
{
    public int AmountSubscribers { get; set; }
    public int AmountAppliedCVs { get; set; }
}
