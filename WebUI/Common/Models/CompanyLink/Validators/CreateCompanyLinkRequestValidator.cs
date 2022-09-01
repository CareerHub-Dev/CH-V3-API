using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.CompanyLink.Validators;

public class CreateCompanyLinkRequestValidator : AbstractValidator<CreateCompanyLinkRequest>
{
    public CreateCompanyLinkRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
