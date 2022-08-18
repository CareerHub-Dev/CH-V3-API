using FluentValidation;

namespace WebUI.DTO.Company.Validators;

public class InviteCompanyRequestValidator : AbstractValidator<InviteCompanyRequest>
{
    public InviteCompanyRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
