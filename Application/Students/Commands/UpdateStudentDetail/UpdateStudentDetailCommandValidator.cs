using Application.Common.Entensions;
using FluentValidation;

namespace Application.Students.Commands.UpdateStudentDetail;

public class UpdateStudentDetailCommandValidator : AbstractValidator<UpdateStudentDetailCommand>
{
    public UpdateStudentDetailCommandValidator()
    {
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(32);

        When(x => x.Phone != null, () =>
        {
            RuleFor(x => x.Phone!)
                .NotEmpty()
                .MaximumLength(32)
                .Phone();
        });

        RuleFor(x => x.BirthDate)
           .LessThan(DateTime.UtcNow).When(x => x.BirthDate.HasValue);
    }
}
