using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetAmount;

public record GetAmountAppliedCVsOfJobOfferWithFilterQuery : IRequest<int>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public ActivationStatus? CompanyOfJobOfferMustHaveActivationStatus { get; init; }

    public bool? IsStudentOfAppliedCVMustBeVerified { get; init; }
    public ActivationStatus? StudentOfCVMustHaveActivationStatus { get; init; }
}

public class GetAmountAppliedCVsOfJobOfferWithFilterQueryHandler
    : IRequestHandler<GetAmountAppliedCVsOfJobOfferWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountAppliedCVsOfJobOfferWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountAppliedCVsOfJobOfferWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.JobOffers
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified,
                companyActivationStatus: request.CompanyOfJobOfferMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .SelectMany(x => x.AppliedCVs)
            .Filter(
                isStudentVerified: request.IsStudentOfAppliedCVMustBeVerified,
                studentActivationStatus: request.StudentOfCVMustHaveActivationStatus
            )
            .CountAsync();
    }
}