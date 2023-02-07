namespace Application.Common.DTO.JobOfferReviews;

public class ShortCVofReviewDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
