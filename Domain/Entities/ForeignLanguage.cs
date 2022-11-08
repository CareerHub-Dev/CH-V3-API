using Domain.Enums;

namespace Domain.Entities;

public class ForeignLanguage
{
    public string Name { get; set; } = string.Empty;
    public LanguageLevel LanguageLevel { get; set; }
}
