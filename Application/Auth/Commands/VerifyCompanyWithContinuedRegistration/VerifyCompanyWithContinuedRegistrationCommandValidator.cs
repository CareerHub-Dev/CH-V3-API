using Application.Common.Entensions;
using FluentValidation;

namespace Application.Auth.Commands.VerifyCompanyWithContinuedRegistration;

public class VerifyCompanyWithContinuedRegistrationCommandValidator : AbstractValidator<VerifyCompanyWithContinuedRegistrationCommand>
{
    public VerifyCompanyWithContinuedRegistrationCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(40);
        RuleFor(x => x.CompanyMotto).NotEmpty().MaximumLength(40);
        RuleFor(x => x.CompanyDescription).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().Password();
    }
}
