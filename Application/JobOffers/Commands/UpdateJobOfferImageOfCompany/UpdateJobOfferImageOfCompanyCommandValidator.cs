using Application.Common.Entensions;
using FluentValidation;

namespace Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;

public class UpdateJobOfferImageOfCompanyCommandValidator
    : AbstractValidator<UpdateJobOfferImageOfCompanyCommand>
{
    public UpdateJobOfferImageOfCompanyCommandValidator()
    {
        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
