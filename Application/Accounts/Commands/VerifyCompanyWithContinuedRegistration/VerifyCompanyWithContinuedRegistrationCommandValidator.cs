using Application.Common.Entensions;
using Application.Common.Models.Image;
using FluentValidation;

namespace Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;

public class VerifyCompanyWithContinuedRegistrationCommandValidator : AbstractValidator<VerifyCompanyWithContinuedRegistrationCommand>
{
    public VerifyCompanyWithContinuedRegistrationCommandValidator(IValidator<CreateImage> imageValidator)
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        When(x => x.Logo != null, () =>
        {
            RuleFor(x => x.Logo)
                .SetValidator(imageValidator!);
        });
        When(x => x.Banner != null, () =>
        {
            RuleFor(x => x.Banner)
                .SetValidator(imageValidator!);
        });

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Motto)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}
