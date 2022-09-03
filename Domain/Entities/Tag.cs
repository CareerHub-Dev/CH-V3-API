using Domain.Common;

namespace Domain.Entities;

public class Tag : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsAccepted { get; set; }

    public List<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
}
