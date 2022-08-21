using Application.Admins.Queries.Models;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Filtration.Admin;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries;

public record GetAdminsQuery : IRequest<PaginatedList<AdminDTO>>
{
    public PaginationParameters PaginationParameters { get; init; } = new PaginationParameters();
    public AdminListFilterParameters? FilterParameters { get; init; }
}

public class GetAdminsQueryHandler : IRequestHandler<GetAdminsQuery, PaginatedList<AdminDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAdminsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<AdminDTO>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Admins.AsNoTracking();

        if(request.FilterParameters != null)
        {
            query = query.Filter(request.FilterParameters);
        }

        return await query
            .Select(x => new AdminDTO
            {
                Id = x.Id,
                Email = x.Email,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset
            })
            .ToPagedListAsync(request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize, cancellationToken);
    }
}