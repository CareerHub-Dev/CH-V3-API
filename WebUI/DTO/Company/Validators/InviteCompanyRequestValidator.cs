﻿using FluentValidation;

namespace WebUI.DTO.Company.Validators;

public class InviteAdminRequestValidator : AbstractValidator<InviteCompanyRequest>
{
    public InviteAdminRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
