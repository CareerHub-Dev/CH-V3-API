using FluentValidation;

namespace WebUI.Common.Models.Admin.Validators;

public class InviteAdminRequestValidator : AbstractValidator<InviteAdminRequest>
{
    public InviteAdminRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
