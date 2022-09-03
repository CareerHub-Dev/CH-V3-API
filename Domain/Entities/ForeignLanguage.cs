using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ForeignLanguage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public LanguageLevel LanguageLevel { get; set; }

    public Guid CVId { get; set; }
    public CV? CV { get; set; }
}
