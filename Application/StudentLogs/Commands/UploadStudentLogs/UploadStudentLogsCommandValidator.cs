using Application.Common.Entensions;
using FluentValidation;

namespace Application.StudentLogs.Commands.UploadStudentLogs;

public class UploadStudentLogsCommandValidator : AbstractValidator<UploadStudentLogsCommand>
{
    public UploadStudentLogsCommandValidator()
    {
        RuleFor(x => x.File)
            .AllowedExtensions(".csv")
            .MaxFileSize(102400);
    }
}
