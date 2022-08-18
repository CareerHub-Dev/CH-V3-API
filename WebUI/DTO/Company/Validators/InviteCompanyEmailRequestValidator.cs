using FluentValidation;

namespace WebUI.DTO.Company.Validators;

public class InviteCompanyEmailRequestValidator : AbstractValidator<InviteCompanyEmailRequest>
{
    public InviteCompanyEmailRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
    }
}
