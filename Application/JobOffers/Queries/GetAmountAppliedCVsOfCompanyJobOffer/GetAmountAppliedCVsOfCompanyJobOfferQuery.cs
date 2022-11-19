using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetAmountAppliedCVsOfCompanyJobOffer;

public record GetAmountAppliedCVsOfCompanyJobOfferQuery 
    : IRequest<int>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }

    public bool? IsStudentOfAppliedCVMustBeVerified { get; init; }
}

public class GetAmountAppliedCVsOfCompanyJobOfferQueryHandler
    : IRequestHandler<GetAmountAppliedCVsOfCompanyJobOfferQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountAppliedCVsOfCompanyJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountAppliedCVsOfCompanyJobOfferQuery request,
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
            .SelectMany(x => x.AppliedCVs)
            .Filter(isStudentVerified: request.IsStudentOfAppliedCVMustBeVerified)
            .CountAsync();
    }
}