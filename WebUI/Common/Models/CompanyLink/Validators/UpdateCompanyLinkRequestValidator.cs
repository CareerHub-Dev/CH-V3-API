using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.CompanyLink.Validators;

public class UpdateCompanyLinkRequestValidator : AbstractValidator<UpdateCompanyLinkRequest>
{
    public UpdateCompanyLinkRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
