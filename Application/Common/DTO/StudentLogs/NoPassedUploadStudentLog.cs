namespace Application.Common.DTO.StudentLogs;

public class NoPassedUploadStudentLog
{
    public UploadStudentLogModel? UploadStudentLog { get; set; }
    public List<string>? Errors { get; set; }
}
