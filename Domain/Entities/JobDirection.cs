using Domain.Enums;

namespace Domain.Entities;

public class JobDirection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TemplateLanguage RecomendedTemplateLanguage { get; set; }
    public List<JobPosition> Positions { get; set; } = new List<JobPosition>();
}
