using Application.Common.Entensions;
using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyLinks;

public class UpdateCompanyLinksCommandValidator : AbstractValidator<UpdateCompanyLinksCommand>
{
	public UpdateCompanyLinksCommandValidator()
	{
        RuleForEach(x => x.Links).ChildRules(link =>
        {
            link.RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(64);

            link.RuleFor(x => x.Uri)
                .NotEmpty()
                .Uri();
        });
    }
}
