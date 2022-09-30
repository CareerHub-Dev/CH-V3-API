using Domain.Enums;

namespace Application.Companies.Queries.Models;

public class StatsFilter
{
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsSubscriberMustBeVerified { get; init; }
    public ActivationStatus? SubscriberMustHaveActivationStatus { get; init; }
}
