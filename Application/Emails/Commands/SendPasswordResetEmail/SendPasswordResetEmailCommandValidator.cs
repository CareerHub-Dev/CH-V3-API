using FluentValidation;

namespace Application.Emails.Commands.SendPasswordResetEmail;

public class SendPasswordResetEmailCommandValidator
    : AbstractValidator<SendPasswordResetEmailCommand>
{
    public SendPasswordResetEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(256)
            .EmailAddress();
    }
}
