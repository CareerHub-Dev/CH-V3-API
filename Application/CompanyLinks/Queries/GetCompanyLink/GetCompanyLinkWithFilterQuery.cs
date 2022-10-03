using Application.Common.DTO.CompanyLinks;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries.GetCompanyLink;

public record GetCompanyLinkWithFilterQuery : IRequest<CompanyLinkDTO>
{
    public Guid CompanyLinkId { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public ActivationStatus CompanyMustHaveActivationStatus { get; init; }
}

public class GetCompanyLinkWithFilterQueryHandler : IRequestHandler<GetCompanyLinkWithFilterQuery, CompanyLinkDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLinkDTO> Handle(GetCompanyLinkWithFilterQuery request, CancellationToken cancellationToken)
    {
        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .Filter(
                isCompanyVerified: request.IsCompanyMustBeVerified,
                companyActivationStatus: request.CompanyMustHaveActivationStatus
            )
            .Where(x => x.Id == request.CompanyLinkId)
            .Select(x => new CompanyLinkDTO
            {
                Id = x.Id,
                Title = x.Title,
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