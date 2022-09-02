using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CompanyLinks.Query.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Query;

public record GetCompanyLinksOfCompanyWithFilterQuery : IRequest<IEnumerable<CompanyLinkDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class GetCompanyLinksOfCompanyWithFilterQueryHandler : IRequestHandler<GetCompanyLinksOfCompanyWithFilterQuery, IEnumerable<CompanyLinkDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinksOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CompanyLinkDTO>> Handle(GetCompanyLinksOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.Filter(IsVerified: request.IsCompanyVerified).AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
            .AsNoTracking()
            .Select(x => new CompanyLinkDTO
            {
                Id = x.Id,
                Name = x.Name,
                Uri = x.Uri,
                CompanyId = x.CompanyId,
                Created = x.Created,
                LastModified = x.LastModified,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToListAsync();
    }
}