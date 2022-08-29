using Application.Common.Entensions;
using FluentValidation;

namespace Application.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Phone();

        RuleFor(x => x.BirthDate)
           .LessThan(DateTime.UtcNow).When(x => x.BirthDate.HasValue);
    }
}
