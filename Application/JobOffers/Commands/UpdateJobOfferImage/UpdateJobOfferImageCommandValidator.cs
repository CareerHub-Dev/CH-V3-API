using Application.Common.Entensions;
using FluentValidation;

namespace Application.JobOffers.Commands.UpdateJobOfferImage;

public class UpdateJobOfferImageCommandValidator
    : AbstractValidator<UpdateJobOfferImageCommand>
{
    public UpdateJobOfferImageCommandValidator()
    {
        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
    }
}
