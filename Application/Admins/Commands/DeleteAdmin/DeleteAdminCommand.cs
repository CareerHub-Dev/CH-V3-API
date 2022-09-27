﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Commands.DeleteAdmin;

public record DeleteAdminCommand(Guid AdminId) : IRequest;

public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccountHelper _accountHelper;

    public DeleteAdminCommandHandler(IApplicationDbContext context, IAccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.AdminId);

        if (admin == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        if (_accountHelper.GetRole(admin) == "SuperAdmin")
        {
            throw new ArgumentException("It is forbidden to delete the super admin.");
        }

        _context.Admins.Remove(admin);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}