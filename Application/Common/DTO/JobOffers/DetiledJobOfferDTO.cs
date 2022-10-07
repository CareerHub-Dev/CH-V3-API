using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Domain.Enums;

namespace Application.Common.DTO.JobOffers;

public class DetiledJobOfferDTO
{
    public Guid Id { get; set; }
    public string Title { set; get; } = string.Empty;
    public Guid? ImageId { set; get; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public JobType JobType { get; set; }
    public WorkFormat WorkFormat { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }

    public JobPositionDTO JobPosition { get; set; } = new JobPositionDTO();

    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
}
