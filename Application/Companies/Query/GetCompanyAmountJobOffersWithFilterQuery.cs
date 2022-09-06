using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyAmountJobOffersWithFilterQuery : IRequest<int>
{
    public Guid CompanyId { get; init; }
    public bool? IsVerified { get; init; }

    public bool? IsJobOfferActive { get; init; }
}

public class GetCompanyAmountJobOffersWithFilterQueryHandler
    : IRequestHandler<GetCompanyAmountJobOffersWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyAmountJobOffersWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetCompanyAmountJobOffersWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(IsVerified: request.IsVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.JobOffers.Filter(request.IsJobOfferActive).CountAsync();
    }
}