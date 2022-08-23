using FluentValidation;

namespace WebUI.Common.Models.Company.Validators;

public class InviteAdminRequestValidator : AbstractValidator<InviteCompanyRequest>
{
    public InviteAdminRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
