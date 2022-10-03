using Application.Common.DTO.CompanyLinks;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries.GetCompanyLinks;

public record GetCompanyLinksOfCompanyWithFilterQuery : IRequest<IEnumerable<CompanyLinkDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
    public ActivationStatus CompanyMustHaveActivationStatus { get; init; }
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
        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyMustBeVerified,
                activationStatus: request.CompanyMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
            .AsNoTracking()
            .Where(x => x.CompanyId == request.CompanyId)
            .OrderBy(x => x.Title)
            .Select(x => new CompanyLinkDTO
            {
                Id = x.Id,
                Title = x.Title,
                Uri = x.Uri,
                CompanyId = x.CompanyId
            })
            .ToListAsync();
    }
}