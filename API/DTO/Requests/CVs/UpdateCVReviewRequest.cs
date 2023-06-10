using Domain.Enums;

namespace API.DTO.Requests.CVs;

public record UpdateCVReviewRequest
{
    public string Message { get; set; } = string.Empty;
    public Review Status { get; set; }
}
