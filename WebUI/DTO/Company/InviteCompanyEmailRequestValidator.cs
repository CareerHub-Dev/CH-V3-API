using FluentValidation;

namespace WebUI.DTO.Company;

public class InviteCompanyEmailRequestValidator : AbstractValidator<InviteCompanyEmailRequest>
{
    public InviteCompanyEmailRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
    }
}
