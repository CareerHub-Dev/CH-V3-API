using Application.Common.Entensions;
using FluentValidation;

namespace Application.CompanyLinks.Command.CreateCompanyLink;

public class CreateCompanyLinkCommandValidator : AbstractValidator<CreateCompanyLinkCommand>
{
    public CreateCompanyLinkCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Uri)
            .NotEmpty()
            .Uri();
    }
}
