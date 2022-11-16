using Application.Common.Entensions;
using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyBanner;

public class UpdateCompanyBannerCommandValidator
    : AbstractValidator<UpdateCompanyBannerCommand>
{
    public UpdateCompanyBannerCommandValidator()
    {
        When(x => x.Banner != null, () =>
        {
            RuleFor(x => x.Banner!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
