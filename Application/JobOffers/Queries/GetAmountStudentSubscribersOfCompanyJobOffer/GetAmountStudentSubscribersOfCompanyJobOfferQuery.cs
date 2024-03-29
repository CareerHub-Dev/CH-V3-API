﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetAmountStudentSubscribersOfCompanyJobOffer;

public record GetAmountStudentSubscribersOfCompanyJobOfferQuery
    : IRequest<int>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public bool? IsSubscriberMustBeVerified { get; init; }
}

public class GetAmountStudentSubscribersOfCompanyJobOfferQueryHandler
    : IRequestHandler<GetAmountStudentSubscribersOfCompanyJobOfferQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountStudentSubscribersOfCompanyJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountStudentSubscribersOfCompanyJobOfferQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(isVerified: request.IsCompanyOfJobOfferMustBeVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (!await _context.JobOffers
            .Filter(isActive: request.IsJobOfferMustBeActive)
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .SelectMany(x => x.SubscribedStudents)
            .Filter(isVerified: request.IsSubscriberMustBeVerified)
            .CountAsync();
    }
}