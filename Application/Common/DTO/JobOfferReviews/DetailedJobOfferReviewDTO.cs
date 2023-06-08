using Application.Common.DTO.Students;
using Domain.Enums;

namespace Application.Common.DTO.JobOfferReviews;

public class DetailedJobOfferReviewDTO
{
    public required Guid Id { get; set; }
    public required Review Status { get; set; }
    public required string? Message { get; set; }
    public required DateTime Created { get; set; }
    public required ShortCVofReviewDTO CV { get; set; }
    public required ShortJobOfferOfReviewDTO JobOffer { get; set; }
    public required DetailedStudentDTO Student {  get; set; }
}
