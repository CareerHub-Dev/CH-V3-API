namespace Application.Common.DTO.JobOffers;

public class JobOfferDTO : DetiledJobOfferDTO
{
    public string Overview { set; get; } = string.Empty;
    public string Requirements { set; get; } = string.Empty;
    public string Responsibilities { set; get; } = string.Empty;
    public string Preferences { set; get; } = string.Empty;
}
