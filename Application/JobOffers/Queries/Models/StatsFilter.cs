namespace Application.JobOffers.Queries.Models;

public record StatsFilter
{
    public bool? IsStudentOfAppliedCVMustBeVerified { get; init; }

    public bool? IsSubscriberMustBeVerified { get; init; }
}
