using FluentValidation;

namespace WebUI.DTO.Admin.Validators;

public class InviteAdminRequestValidator : AbstractValidator<InviteAdminRequest>
{
    public InviteAdminRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
