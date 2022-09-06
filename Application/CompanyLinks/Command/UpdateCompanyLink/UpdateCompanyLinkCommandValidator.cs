using Application.Common.Entensions;
using FluentValidation;

namespace Application.CompanyLinks.Command.UpdateCompanyLink;

public class UpdateCompanyLinkCommandValidator : AbstractValidator<UpdateCompanyLinkCommand>
{
    public UpdateCompanyLinkCommandValidator()
    {
        RuleFor(c => c.Name)
           .NotEmpty()
           .MaximumLength(32);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
