using Application.Common.Entensions;
using FluentValidation;
using WebUI.Common.Extentions;

namespace WebUI.Common.Models.Account.Validators;

public class VerifyCompanyWithContinuedRegistrationRequestValidator : AbstractValidator<VerifyCompanyWithContinuedRegistrationRequest>
{
    public VerifyCompanyWithContinuedRegistrationRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        When(x => x.LogoFile != null, () =>
        {
            RuleFor(x => x.LogoFile!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });

        When(x => x.BannerFile != null, () =>
        {
            RuleFor(x => x.BannerFile!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.CompanyMotto)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.CompanyDescription)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
