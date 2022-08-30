using FluentValidation;

namespace WebUI.Common.Models.Company.Validators;

public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.CompanyMotto)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.CompanyDescription)
            .NotEmpty()
            .MaximumLength(256);
    }
}
