using Domain.Enums;

namespace Domain.Entities;

public class CVJobOffer
{
    public Guid CVId { get; set; }
    public CV? CV { get; set; }

    public Guid JobOfferId { get; set; }
    public JobOffer? JobOffer { get; set; }

    public Review Status { get; set; } = Review.In_progress;
}
