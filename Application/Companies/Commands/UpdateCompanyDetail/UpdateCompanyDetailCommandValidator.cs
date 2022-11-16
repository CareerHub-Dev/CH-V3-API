using FluentValidation;

namespace Application.Companies.Commands.UpdateCompanyDetail;

public class UpdateCompanyDetailCommandValidator
    : AbstractValidator<UpdateCompanyDetailCommand>
{
    public UpdateCompanyDetailCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Motto)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1024);
    }
}
