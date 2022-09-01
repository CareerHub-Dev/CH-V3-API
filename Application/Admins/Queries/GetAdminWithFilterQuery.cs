using Application.Admins.Queries.Models;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries;

public record GetAdminWithFilterQuery : IRequest<AdminDTO>
{
    public Guid AdminId { get; init; }

    public bool? IsVerified { get; init; }
}

public class GetAdminWithFilterQueryHandler : IRequestHandler<GetAdminWithFilterQuery, AdminDTO>
{
    private readonly IApplicationDbContext _context;

    public GetAdminWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDTO> Handle(GetAdminWithFilterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins
            .AsNoTracking()
            .Where(x => x.Id == request.AdminId)
            .Filter(IsVerified: request.IsVerified)
            .Select(x => new AdminDTO
            {
                Id = x.Id,
                Email = x.Email,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        return entity;
    }
}