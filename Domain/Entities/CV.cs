using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class CV : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public Guid JobPositionId { get; set; }
    public JobPosition? JobPosition { get; set; }

    public TemplateLanguage TemplateLanguage { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; }
    public string Goals { get; set; } = string.Empty;
    public string SkillsAndTechnologies { get; set; } = string.Empty;
    public string OtherExperience { get; set; } = string.Empty;

    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }

    public Guid StudentId { get; set; }
    public Student? Student { get; set; }

    public List<JobOffer> TargetJobOffers { get; set; } = new List<JobOffer>();
    public List<ForeignLanguage> ForeignLanguages { get; set; } = new List<ForeignLanguage>();
    public List<Experience> Experiences { get; set; } = new List<Experience>();
    public List<CVProjectLink> ProjectLinks { get; set; } = new List<CVProjectLink>();
    public List<Education> Educations { get; set; } = new List<Education>();
}
