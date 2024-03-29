﻿namespace Application.Common.DTO.Tags;

public class TagDTO : BriefTagDTO
{
    public bool IsAccepted { get; set; }
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
