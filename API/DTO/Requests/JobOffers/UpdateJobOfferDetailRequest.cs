using Domain.Enums;

namespace API.DTO.Requests.JobOffers;

public record UpdateJobOfferDetailRequest
{
    public Guid JobOfferId { get; init; }

    public string Title { init; get; } = string.Empty;
    public string Overview { init; get; } = string.Empty;
    public string Requirements { init; get; } = string.Empty;
    public string Responsibilities { init; get; } = string.Empty;
    public string Preferences { init; get; } = string.Empty;

    public JobType JobType { get; init; }
    public WorkFormat WorkFormat { get; init; }
    public ExperienceLevel ExperienceLevel { get; init; }

    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Guid JobPositionId { get; init; }

    public List<Guid> TagIds { get; init; } = new List<Guid>();
}
