using Domain.Common;

namespace Domain.Entities;

public class JobPosition : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid JobDirectionId { get; set; }
    public JobDirection? JobDirection { get; set; }

    public List<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
    public List<CV> CVs { get; set; } = new List<CV>();
}
