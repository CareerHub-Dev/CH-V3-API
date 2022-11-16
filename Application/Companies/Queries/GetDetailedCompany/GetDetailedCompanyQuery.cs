using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetDetailedCompany;

public record GetDetailedCompanyQuery
    : IRequest<DetailedCompanyDTO>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetDetailedCompanyQueryHandler
    : IRequestHandler<GetDetailedCompanyQuery, DetailedCompanyDTO>
{
    private readonly IApplicationDbContext _context;

    public GetDetailedCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DetailedCompanyDTO> Handle(
        GetDetailedCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Where(x => x.Id == request.CompanyId)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .MapToDetailedCompanyDTO()
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return company;
    }
}