using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.ForeignLanguages;
using Domain.Enums;

namespace API.DTO.Requests.CVs;

public record UpdateOwnCVDetailRequest
{
    public Guid CVId { get; init; }

    public string Title { get; init; } = string.Empty;

    public Guid JobPositionId { get; init; }

    public TemplateLanguage TemplateLanguage { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Goals { get; init; } = string.Empty;
    public string SkillsAndTechnologies { get; init; } = string.Empty;
    public string ExperienceHighlights { get; init; } = string.Empty;

    public List<ForeignLanguageDTO> ForeignLanguages { get; init; } = new List<ForeignLanguageDTO>();
    public List<CVProjectLinkDTO> ProjectLinks { get; init; } = new List<CVProjectLinkDTO>();
    public List<EducationDTO> Educations { get; init; } = new List<EducationDTO>();
}
