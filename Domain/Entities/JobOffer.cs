using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class JobOffer : BaseEntity
{
    public string Title { set; get; } = string.Empty;
    public string Overview { set; get; } = string.Empty;
    public string Requirements { set; get; } = string.Empty;
    public string Responsibilities { set; get; } = string.Empty;
    public string Preferences { set; get; } = string.Empty;
    public Guid? ImageId { set; get; }

    public JobType JobType { get; set; }
    public WorkFormat WorkFormat { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public Guid JobPositionId { get; set; }
    public JobPosition? JobPosition { get; set; }

    public List<Student> SubscribedStudents { get; set; } = new List<Student>();
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<CV> AppliedCVs { get; set; } = new List<CV>();
}
