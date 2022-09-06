using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CompanyLinks.Query.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Query;

public record GetCompanyLinkBriefsOfCompanyWithFilterQuery : IRequest<IEnumerable<CompanyLinkBriefDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class GetCompanyLinkBriefsOfCompanyWithFilterQueryHandler : IRequestHandler<GetCompanyLinkBriefsOfCompanyWithFilterQuery, IEnumerable<CompanyLinkBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkBriefsOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CompanyLinkBriefDTO>> Handle(GetCompanyLinkBriefsOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.Filter(isVerified: request.IsCompanyVerified).AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
            .AsNoTracking()
            .Select(x => new CompanyLinkBriefDTO
            {
                Id = x.Id,
                Name = x.Title,
                Uri = x.Uri,
                CompanyId = x.CompanyId
            })
            .ToListAsync();
    }
}