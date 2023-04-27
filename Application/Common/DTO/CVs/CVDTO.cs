using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.DTO.JobDirection;
using Application.Common.DTO.JobPositions;
using Domain.Enums;

namespace Application.Common.DTO.CVs;

public class CVDTO : BriefCVDTO
{
    public JobPositionDTO JobPosition { get; set; } = new JobPositionDTO();
    public JobDirectionDTO JobDirection { get; set; } = new JobDirectionDTO();

    public TemplateLanguage TemplateLanguage { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string Goals { get; set; } = string.Empty;
    public List<string> HardSkills { get; set; } = new List<string>();
    public List<string> SoftSkills { get; set; } = new List<string>();

    public Guid StudentId { get; set; }

    public List<ForeignLanguageDTO> ForeignLanguages { get; set; } = new List<ForeignLanguageDTO>();
    public List<CVProjectLinkDTO> ProjectLinks { get; set; } = new List<CVProjectLinkDTO>();
    public List<EducationDTO> Educations { get; set; } = new List<EducationDTO>();
    public List<CVExperienceDTO> Experiences { get; set; } = new List<CVExperienceDTO>();
}
