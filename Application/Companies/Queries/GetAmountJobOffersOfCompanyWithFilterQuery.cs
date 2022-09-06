using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries;

public record GetAmountJobOffersOfCompanyWithFilterQuery : IRequest<int>
{
    public Guid CompanyId { get; init; }
    public bool? IsVerified { get; init; }

    public bool? IsJobOfferActive { get; init; }
}

public class GetAmountJobOffersOfCompanyWithFilterQueryHandler
    : IRequestHandler<GetAmountJobOffersOfCompanyWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountJobOffersOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountJobOffersOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(isVerified: request.IsVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.JobOffers.Filter(isActive: request.IsJobOfferActive).CountAsync();
    }
}