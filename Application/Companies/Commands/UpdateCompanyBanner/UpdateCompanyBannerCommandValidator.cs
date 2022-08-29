using Application.Common.Models.Image;
using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyBanner;

public class UpdateCompanyBannerCommandValidator : AbstractValidator<UpdateCompanyBannerCommand>
{
    public UpdateCompanyBannerCommandValidator(IValidator<CreateImage> imageValidator)
    {
        When(x => x.Banner != null, () =>
        {
            RuleFor(x => x.Banner)
                .SetValidator(imageValidator!);
        });
    }
}
