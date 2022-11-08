namespace Application.Companies.Queries.Models;

public record StatsFilter
{
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsSubscriberMustBeVerified { get; init; }
}
