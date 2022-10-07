﻿using Domain.Enums;

namespace Application.Companies.Queries.Models;

public record StatsFilter
{
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsSubscriberMustBeVerified { get; init; }
    public ActivationStatus? SubscriberMustHaveActivationStatus { get; init; }
}
