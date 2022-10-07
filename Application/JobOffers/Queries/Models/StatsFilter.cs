using Domain.Enums;

namespace Application.JobOffers.Queries.Models;

public record StatsFilter
{
    public bool? IsStudentOfCVMustBeVerified { get; init; }
    public ActivationStatus? StudentOfCVMustHaveActivationStatus { get; init; }

    public bool? IsSubscriberMustBeVerified { get; init; }
    public ActivationStatus? SubscriberMustHaveActivationStatus { get; init; }
}
