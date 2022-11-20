using Application.Common.Entensions;
using FluentValidation;

namespace Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;

public class VerifyCompanyWithContinuedRegistrationCommandValidator
    : AbstractValidator<VerifyCompanyWithContinuedRegistrationCommand>
{
    public VerifyCompanyWithContinuedRegistrationCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        When(x => x.Logo != null, () =>
        {
            RuleFor(x => x.Logo!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });
        When(x => x.Banner != null, () =>
        {
            RuleFor(x => x.Banner!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Motto)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1024);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();

        RuleForEach(x => x.Links).ChildRules(link =>
        {
            link.RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(64);

            link.RuleFor(x => x.Uri)
                .NotEmpty()
                .Uri();
        });

        RuleFor(x => x.Links.Count)
            .LessThanOrEqualTo(5)
            .OverridePropertyName("Links");
    }
}
