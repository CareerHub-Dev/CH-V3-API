using Application.Common.Models.Image;
using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyLogo;

public class UpdateCompanyLogoCommandValidator : AbstractValidator<UpdateCompanyLogoCommand>
{
    public UpdateCompanyLogoCommandValidator(IValidator<CreateImage> imageValidator)
    {
        When(x => x.Logo != null, () =>
        {
            RuleFor(x => x.Logo)
                .SetValidator(imageValidator!);
        });
    }
}
