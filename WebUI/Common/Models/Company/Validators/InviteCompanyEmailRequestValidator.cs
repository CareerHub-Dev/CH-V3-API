using FluentValidation;

namespace WebUI.Common.Models.Company.Validators;

public class InviteCompanyEmailRequestValidator : AbstractValidator<SendInviteCompanyEmailRequest>
{
    public InviteCompanyEmailRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
    }
}
