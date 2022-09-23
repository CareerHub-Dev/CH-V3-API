using Application.Common.DTO.CompanyLinks;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries.GetCompanyLink;

public record GetCompanyLinkQuery : IRequest<CompanyLinkDTO>
{
    public Guid CompanyLinkId { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetCompanyLinkQueryHandler : IRequestHandler<GetCompanyLinkQuery, CompanyLinkDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLinkDTO> Handle(GetCompanyLinkQuery request, CancellationToken cancellationToken)
    {
        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .Filter(isCompanyVerified: request.IsCompanyMustBeVerified)
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