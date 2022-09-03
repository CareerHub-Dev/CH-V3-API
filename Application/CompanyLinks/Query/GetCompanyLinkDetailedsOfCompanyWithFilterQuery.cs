using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CompanyLinks.Query.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Query;

public record GetCompanyLinkDetailedsOfCompanyWithFilterQuery : IRequest<IEnumerable<CompanyLinkDetailedDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class GetCompanyLinkDetailedsOfCompanyWithFilterQueryHandler : IRequestHandler<GetCompanyLinkDetailedsOfCompanyWithFilterQuery, IEnumerable<CompanyLinkDetailedDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkDetailedsOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CompanyLinkDetailedDTO>> Handle(GetCompanyLinkDetailedsOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.Filter(IsVerified: request.IsCompanyVerified).AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
            .AsNoTracking()
            .Select(x => new CompanyLinkDetailedDTO
            {
                Id = x.Id,
                Name = x.Title,
                Uri = x.Uri,
                CompanyId = x.CompanyId
            })
            .ToListAsync();
    }
}