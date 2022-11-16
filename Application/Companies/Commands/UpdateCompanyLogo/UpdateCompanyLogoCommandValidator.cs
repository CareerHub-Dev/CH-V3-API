using Application.Common.Entensions;
using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyLogo;

public class UpdateCompanyLogoCommandValidator
    : AbstractValidator<UpdateCompanyLogoCommand>
{
    public UpdateCompanyLogoCommandValidator()
    {
        When(x => x.Logo != null, () =>
        {
            RuleFor(x => x.Logo!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
