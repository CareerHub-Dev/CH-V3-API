using Domain.Enums;

namespace Application.Common.DTO.ForeignLanguages;

public class ForeignLanguageDTO
{
    public string Name { get; set; } = string.Empty;
    public LanguageLevel LanguageLevel { get; set; }
}
