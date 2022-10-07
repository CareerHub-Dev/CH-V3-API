using Application.Common.DTO.CompanyLinks;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries.GetCompanyLink;

public record GetCompanyLinkOfCompanyWithFilterQuery : IRequest<CompanyLinkDTO>
{
    public Guid CompanyLinkId { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
    public ActivationStatus CompanyMustHaveActivationStatus { get; init; }
}

public class GetCompanyLinkOfCompanyWithFilterQueryHandler : IRequestHandler<GetCompanyLinkOfCompanyWithFilterQuery, CompanyLinkDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLinkDTO> Handle(GetCompanyLinkOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
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

        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyLinkId && x.CompanyId == request.CompanyId)
            .Select(x => new CompanyLinkDTO
            {
                Id = x.Id,
                Title = x.Title,
                Uri = x.Uri
            })
            .FirstOrDefaultAsync();

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.CompanyLinkId);
        }

        return companyLink;
    }
}