namespace API.DTO.Requests.CVs;

public record SendCVForJobOfferRequest
{
    public Guid CVId { get; init; }
    public Guid JobOfferId { get; init; }
}
