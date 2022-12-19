using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.DTO.JobPositions;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Common.DTO.CVs;

public class CVDTO : BriefCVDTO
{
    public BriefJobPositionDTO JobPosition { get; set; } = new BriefJobPositionDTO();

    public TemplateLanguage TemplateLanguage { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string Goals { get; set; } = string.Empty;
    public string SkillsAndTechnologies { get; set; } = string.Empty;
    public string ExperienceHighlights { get; set; } = string.Empty;

    public Guid StudentId { get; set; }

    public List<ForeignLanguageDTO> ForeignLanguages { get; set; } = new List<ForeignLanguageDTO>();
    public List<CVProjectLinkDTO> ProjectLinks { get; set; } = new List<CVProjectLinkDTO>();
    public List<EducationDTO> Educations { get; set; } = new List<EducationDTO>();
}
