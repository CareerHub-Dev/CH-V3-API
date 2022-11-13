using Application.Common.DTO.Admins;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries.GetAdmins;

public record GetAdminsWithPaginationWithSearchWithFilterWithSortQuery : IRequest<PaginatedList<AdminDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsAdminMustBeVerified { get; init; }
    public Guid? WithoutAdminId { get; init; }
    public bool? IsSuperAdmin { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetAdminsWithPaginationWithSearchWithFilterWithSortQueryHandler : IRequestHandler<GetAdminsWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<AdminDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAdminsWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<AdminDTO>> Handle(GetAdminsWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.Admins
            .AsNoTracking()
            .Filter(
                withoutAdminId: request.WithoutAdminId,
                isVerified: request.IsAdminMustBeVerified,
                isSuperAdmin: request.IsSuperAdmin
             )
            .Search(request.SearchTerm)
            .MapToAdminDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}