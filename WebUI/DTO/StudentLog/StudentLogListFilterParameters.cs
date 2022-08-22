namespace WebUI.DTO.StudentLog;

public class StudentLogListFilterParameters
{
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? StudentGroupId { get; set; }
}
