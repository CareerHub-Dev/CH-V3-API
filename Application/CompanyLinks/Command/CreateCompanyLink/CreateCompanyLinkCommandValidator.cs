using Application.Common.Entensions;
using FluentValidation;

namespace Application.CompanyLinks.Command.CreateCompanyLink;

public class CreateCompanyLinkCommandValidator : AbstractValidator<CreateCompanyLinkCommand>
{
    public CreateCompanyLinkCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
