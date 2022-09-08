using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries;

public record CompanyOwnsJobOfferQuery : IRequest<bool>
{
    public Guid CompanyId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class CompanyOwnsJobOfferQueryHandler : IRequestHandler<CompanyOwnsJobOfferQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public CompanyOwnsJobOfferQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CompanyOwnsJobOfferQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.JobOffers
                .AnyAsync(x => x.CompanyId == request.CompanyId && x.Id == request.JobOfferId);
    }
}