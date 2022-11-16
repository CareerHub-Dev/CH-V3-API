using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyQuery
    : IRequest<CompanyDTO>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, CompanyDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyDTO> Handle(
        GetCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Where(x => x.Id == request.CompanyId)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .MapToCompanyDTO()
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return company;
    }
}