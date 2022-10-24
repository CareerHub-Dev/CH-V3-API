using Application.Common.DTO.Companies;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Domain.Enums;

namespace Application.Common.DTO.JobOffers;

public class DetiledJobOfferDTO
{
    public Guid Id { get; set; }
    public string Title { set; get; } = string.Empty;
    public string? Image { set; get; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public JobType JobType { get; set; }
    public WorkFormat WorkFormat { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }

    public BriefJobPositionDTO JobPosition { get; set; } = new BriefJobPositionDTO();

    public BriefCompanyDTO Company { get; set; } = new BriefCompanyDTO();

    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
}
