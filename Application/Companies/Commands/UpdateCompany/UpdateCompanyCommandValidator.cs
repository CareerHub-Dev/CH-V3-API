using FluentValidation;

namespace Application.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
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
