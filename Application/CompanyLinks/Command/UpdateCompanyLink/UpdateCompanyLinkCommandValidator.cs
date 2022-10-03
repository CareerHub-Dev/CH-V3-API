using Application.Common.Entensions;
using FluentValidation;

namespace Application.CompanyLinks.Command.UpdateCompanyLink;

public class UpdateCompanyLinkCommandValidator : AbstractValidator<UpdateCompanyLinkCommand>
{
    public UpdateCompanyLinkCommandValidator()
    {
        RuleFor(c => c.Title)
           .NotEmpty()
           .MaximumLength(64);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
