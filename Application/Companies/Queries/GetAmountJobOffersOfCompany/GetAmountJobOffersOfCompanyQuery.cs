using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetAmountJobOffersOfCompany;

public record GetAmountJobOffersOfCompanyQuery
    : IRequest<int>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }

    public bool? IsJobOfferMustBeActive { get; init; }
}

public class GetAmountJobOffersOfCompanyWithFilterQueryHandler
    : IRequestHandler<GetAmountJobOffersOfCompanyQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountJobOffersOfCompanyWithFilterQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountJobOffersOfCompanyQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.JobOffers
            .Where(x => x.CompanyId == request.CompanyId)
            .Filter(isActive: request.IsJobOfferMustBeActive)
            .CountAsync();
    }
}