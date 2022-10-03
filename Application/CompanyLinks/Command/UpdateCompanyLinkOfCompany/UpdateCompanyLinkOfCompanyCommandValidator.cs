using Application.Common.Entensions;
using FluentValidation;

namespace Application.CompanyLinks.Command.UpdateCompanyLinkOfCompany;

public class UpdateCompanyLinkOfCompanyCommandValidator : AbstractValidator<UpdateCompanyLinkOfCompanyCommand>
{
    public UpdateCompanyLinkOfCompanyCommandValidator()
    {
        RuleFor(c => c.Title)
           .NotEmpty()
           .MaximumLength(64);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
