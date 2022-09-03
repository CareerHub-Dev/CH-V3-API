using Domain.Common;

namespace Domain.Entities;

public class JobPosition : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public List<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
    public List<CV> CVs { get; set; } = new List<CV>();
}
