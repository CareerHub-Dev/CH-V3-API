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
