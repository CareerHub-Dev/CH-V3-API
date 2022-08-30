using FluentValidation;
using WebUI.Common.Extentions;

namespace WebUI.Common.Models.Company.Validators;

public class UpdateCompanyBannerRequestValidator : AbstractValidator<UpdateCompanyBannerRequest>
{
    public UpdateCompanyBannerRequestValidator()
    {
        When(x => x.BannerFile != null, () =>
        {
            RuleFor(x => x.BannerFile!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
