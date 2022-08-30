using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.Student.Validators;

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(30);

        When(x => x.Phone != null, () =>
        {
            RuleFor(x => x.Phone!)
                .NotEmpty()
                .Phone();
        });

        RuleFor(x => x.BirthDate)
           .LessThan(DateTime.UtcNow).When(x => x.BirthDate.HasValue);
    }
}
