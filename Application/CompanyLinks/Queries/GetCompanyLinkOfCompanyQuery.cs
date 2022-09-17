using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries;

public record GetCompanyLinkOfCompanyQuery : IRequest<CompanyLinkDTO>
{
    public Guid CompanyLinkId { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetCompanyLinkOfCompanyQueryHandler : IRequestHandler<GetCompanyLinkOfCompanyQuery, CompanyLinkDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkOfCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLinkDTO> Handle(GetCompanyLinkOfCompanyQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.Filter(isVerified: request.IsCompanyMustBeVerified).AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyLinkId)
            .Select(x => new CompanyLinkDTO
            {
                Id = x.Id,
                Name = x.Title,
                Uri = x.Uri,
                CompanyId = x.CompanyId
            })
            .FirstOrDefaultAsync();

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.CompanyLinkId);
        }

        return companyLink;
    }
}