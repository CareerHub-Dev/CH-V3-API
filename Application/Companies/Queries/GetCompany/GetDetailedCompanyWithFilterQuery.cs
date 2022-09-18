﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetDetailedCompanyWithFilterQuery : IRequest<DetailedCompanyDTO>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetDetailedCompanyWithFilterQueryHandler
    : IRequestHandler<GetDetailedCompanyWithFilterQuery, DetailedCompanyDTO>
{
    private readonly IApplicationDbContext _context;

    public GetDetailedCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DetailedCompanyDTO> Handle(
        GetDetailedCompanyWithFilterQuery request, 
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyId)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .Select(x => new DetailedCompanyDTO
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