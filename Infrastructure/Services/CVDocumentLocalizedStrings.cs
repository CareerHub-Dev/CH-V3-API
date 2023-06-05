using Domain.Enums;

namespace Infrastructure.Services;

public record CvDocumentLocalizedStrings
{
    public string HardSkills { get; init; } = "Hard Skills";
    public string SoftSkills { get; init; } = "Soft Skills";
    public string ForeignLanguages { get; init; } = "Foreign languages";
    public string Education { get; init; } = "Education";
    public string Experience { get; init; } = "Working experience";
    public string ProjectLinks { get; init; } = "Projects";
    public string MyPhoto { get; init; } = "My photo";
    public string Present { get; init; } = "present";
    public string At { get; init; } = "at";

    public static CvDocumentLocalizedStrings FromTemplateLanguage(TemplateLanguage templateLanguage)
    {
        return templateLanguage switch
        {
            TemplateLanguage.EN => EN,
            TemplateLanguage.UA => UK,
            _ => throw new ArgumentOutOfRangeException(nameof(templateLanguage), templateLanguage, null)
        };
    }

    public static readonly CvDocumentLocalizedStrings EN = new ();
    public static readonly CvDocumentLocalizedStrings UK = new () 
    { 
        HardSkills = "Технічні навички", 
        SoftSkills = "Персональні навички", 
        ForeignLanguages = "Іноземні мови", 
        Education = "Освіта", 
        Experience = "Досвід роботи", 
        ProjectLinks = "Проекти", 
        MyPhoto = "Моє фото", 
        Present = "нині",
        At = "в",
    };
}


