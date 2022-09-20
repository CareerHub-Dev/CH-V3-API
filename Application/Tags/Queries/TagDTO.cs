using Application.Common.Models.Tag;

namespace Application.Tags.Queries;

public class TagDTO : BriefTagDTO
{
    public bool IsAccepted { get; set; }
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
