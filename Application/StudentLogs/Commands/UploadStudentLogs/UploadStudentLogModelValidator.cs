using Application.Common.DTO.StudentLogs;
using Application.Common.Entensions;
using FluentValidation;

namespace CareerHub.BLL.Validation.StudentLog
{
    public class UploadStudentLogModelValidator : AbstractValidator<UploadStudentLogModel>
    {
        public UploadStudentLogModelValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Email).NotEmpty().NureEmailAddress();
            RuleFor(x => x.Group).NotEmpty().MaximumLength(20);
        }
    }
}
