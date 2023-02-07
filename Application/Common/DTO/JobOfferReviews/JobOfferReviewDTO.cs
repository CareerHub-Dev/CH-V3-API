using Domain.Enums;

namespace Application.Common.DTO.JobOfferReviews;

public class JobOfferReviewDTO
{
    public Review Status { get; set; }
    public string? Message { get; set; }
    public DateTime Created { get; set; }
    public ShortCVofReviewDTO CV { get; set; } = new ShortCVofReviewDTO();
    public ShortJobOfferOfReviewDTO JobOffer { get; set; } = new ShortJobOfferOfReviewDTO();
}
