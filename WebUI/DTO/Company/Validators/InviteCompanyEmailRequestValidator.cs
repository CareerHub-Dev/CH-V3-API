using FluentValidation;

namespace WebUI.DTO.Company.Validators;

public class InviteCompanyEmailRequestValidator : AbstractValidator<SendInviteCompanyEmailRequest>
{
    public InviteCompanyEmailRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
    }
}
