using Application.Common.Entensions;
using FluentValidation;

namespace WebUI.Common.Models.StudentLog.Validators
{
    public class UpdateStudentLogRequestValidator : AbstractValidator<UpdateStudentLogRequest>
    {
        public UpdateStudentLogRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NureEmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
