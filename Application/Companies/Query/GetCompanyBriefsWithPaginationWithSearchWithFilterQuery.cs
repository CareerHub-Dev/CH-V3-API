using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Query.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyBriefsWithPaginationWithSearchWithFilterQuery 
    : IRequest<PaginatedList<CompanyBriefDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }
}

public class GetCompanyBriefsWithPaginationWithSearchWithFilterQueryHandler 
    : IRequestHandler<GetCompanyBriefsWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyBriefsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyBriefDTO>> Handle(GetCompanyBriefsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(request.WithoutCompanyId, request.IsVerified)
            .Search(request.SearchTerm ?? "")
            .Select(x => new CompanyBriefDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}