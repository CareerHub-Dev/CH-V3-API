﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries;

public record GetAdminQuery(Guid AdminId) : IRequest<AdminDTO>;

public class GetAdminQueryHandler : IRequestHandler<GetAdminQuery, AdminDTO>
{
    private readonly IApplicationDbContext _context;
    public GetAdminQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDTO> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .AsNoTracking()
            .Where(x => x.Id == request.AdminId)
            .Select(x => new AdminDTO
            {
                Id = x.Id,
                Email = x.Email,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                IsSuperAdmin = x.IsSuperAdmin
            })
            .FirstOrDefaultAsync();

        if (admin == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        return admin;
    }
}