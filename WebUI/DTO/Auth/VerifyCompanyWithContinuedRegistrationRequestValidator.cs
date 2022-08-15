using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.DTO.Auth;

public class VerifyCompanyWithContinuedRegistrationRequestValidator : AbstractValidator<VerifyCompanyWithContinuedRegistrationRequest>
{
    public VerifyCompanyWithContinuedRegistrationRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CompanyMotto).NotEmpty().MaximumLength(128);
        RuleFor(x => x.CompanyDescription).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().Password();
    }
}
