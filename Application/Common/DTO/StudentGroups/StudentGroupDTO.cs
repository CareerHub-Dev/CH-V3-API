﻿namespace Application.Common.DTO.StudentGroups;

public class StudentGroupDTO : BriefStudentGroupDTO
{
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
