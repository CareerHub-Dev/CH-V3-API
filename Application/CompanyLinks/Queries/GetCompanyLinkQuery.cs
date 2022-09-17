using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Queries;

public record GetCompanyLinkQuery : IRequest<CompanyLinkDTO>
{
    public Guid CompanyLinkId { get; init; }
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