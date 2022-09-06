using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries;

public record CompanyOwnsCompanyLinkQuery : IRequest<bool>
{
    public Guid CompanyId { get; init; }
    public Guid CompanyLinkId { get; init; }
}

public class CompanyOwnsCompanyLinkQueryHandler : IRequestHandler<CompanyOwnsCompanyLinkQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public CompanyOwnsCompanyLinkQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CompanyOwnsCompanyLinkQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
                .AnyAsync(x => x.CompanyId == request.CompanyId && x.Id == request.CompanyLinkId);
    }
}