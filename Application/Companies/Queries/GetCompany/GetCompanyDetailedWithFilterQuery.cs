using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyDetailedWithFilterQuery : IRequest<CompanyDetailedDTO>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetCompanyDetailedWithFilterQueryHandler
    : IRequestHandler<GetCompanyDetailedWithFilterQuery, CompanyDetailedDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyDetailedWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyDetailedDTO> Handle(GetCompanyDetailedWithFilterQuery request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyId)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .Select(x => new CompanyDetailedDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
            })
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return company;
    }
}