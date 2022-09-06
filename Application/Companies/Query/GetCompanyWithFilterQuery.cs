using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Companies.Query.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyWithFilterQuery : IRequest<CompanyDTO>
{
    public Guid CompanyId { get; init; }
    public bool? IsVerified { get; init; }
}

public class GetCompanyWithFilterQueryHandler
    : IRequestHandler<GetCompanyWithFilterQuery, CompanyDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyDTO> Handle(GetCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyId)
            .Filter(IsVerified: request.IsVerified)
            .Select(x => new CompanyDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return company;
    }
}