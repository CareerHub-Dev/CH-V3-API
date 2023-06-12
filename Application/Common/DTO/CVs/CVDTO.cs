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
    public required JobPositionDTO JobPosition { get; set; }
    public required JobDirectionDTO JobDirection { get; set; }
    public required TemplateLanguage TemplateLanguage { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public required string? Photo { get; set; }
    public required string Goals { get; set; }
    public required List<string> HardSkills { get; set; }
    public required List<string> SoftSkills { get; set; }
    public required Guid StudentId { get; set; }
    public required List<ForeignLanguageDTO> ForeignLanguages { get; set; }
    public required List<CVProjectLinkDTO> ProjectLinks { get; set; }
    public required List<EducationDTO> Educations { get; set; }
    public required List<CVExperienceDTO> Experiences { get; set; }
}
