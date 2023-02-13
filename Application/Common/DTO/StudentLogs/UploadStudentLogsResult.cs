namespace Application.Common.DTO.StudentLogs;

public class UploadStudentLogsResult
{
    public int Total { get; set; }
    public int AddedAmount { get; set; }
    public int InvalidAmount { get; set; }
    public int UpdatedAmount { get; set; }

    public List<NoPassedUploadStudentLog> NoPassedUploadStudentLogs { get; set; } = new List<NoPassedUploadStudentLog>();
}
