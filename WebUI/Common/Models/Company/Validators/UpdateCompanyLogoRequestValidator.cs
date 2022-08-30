using FluentValidation;
using WebUI.Common.Extentions;

namespace WebUI.Common.Models.Company.Validators;

public class UpdateCompanyLogoRequestValidator : AbstractValidator<UpdateCompanyLogoRequest>
{
    public UpdateCompanyLogoRequestValidator()
    {
        When(x => x.LogoFile != null, () =>
        {
            RuleFor(x => x.LogoFile!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}