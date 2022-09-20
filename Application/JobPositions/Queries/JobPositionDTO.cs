using Application.Common.Models.JobPosition;

namespace Application.JobPositions.Queries;

public class JobPositionDTO : BriefJobPositionDTO
{
    public DateTime Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
