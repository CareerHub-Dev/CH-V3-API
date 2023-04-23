using Domain.Enums;

namespace Application.Common.DTO.JobDirection;

public class JobDirectionDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TemplateLanguage RecomendedTemplateLanguage { get; set; }
}
