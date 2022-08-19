using Application.Admins.Queries.Models;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Filtration.Admin;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries;

public record GetAdminQuery : IRequest<AdminDTO>
{
    public Guid AdminId { get; init; }
    public AdminFilterParameters? FilterParameters { get; init; }
}

public class GetAdminQueryHandler : IRequestHandler<GetAdminQuery, AdminDTO>
{
    private readonly IApplicationDbContext _context;

    public GetAdminQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDTO> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Admins.AsNoTracking().Where(x => x.Id == request.AdminId);

        if (request.FilterParameters != null)
        {
            query = query.Filter(request.FilterParameters);
        }

        var response = await query
            .Select(x => new AdminDTO
            {
                Id = x.Id,
                Email = x.Email,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset
            })
            .FirstOrDefaultAsync(cancellationToken);

        if(response == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        return response;
    }
}