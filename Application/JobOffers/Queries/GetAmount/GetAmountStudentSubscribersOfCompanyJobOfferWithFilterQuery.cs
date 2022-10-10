using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetAmount;

public record GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQuery : IRequest<int>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public ActivationStatus? CompanyOfJobOfferMustHaveActivationStatus { get; init; }

    public bool? IsSubscriberMustBeVerified { get; init; }
    public ActivationStatus? SubscriberMustHaveActivationStatus { get; init; }
}

public class GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQueryHandler
    : IRequestHandler<GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyOfJobOfferMustBeVerified,
                activationStatus: request.CompanyOfJobOfferMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (!await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive
            )
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .SelectMany(x => x.SubscribedStudents)
            .Filter(
                isVerified: request.IsSubscriberMustBeVerified,
                activationStatus: request.SubscriberMustHaveActivationStatus
            )
            .CountAsync();
    }
}