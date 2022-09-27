﻿using Application.Common.DTO.Admins;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Queries.GetAdmins;

public record GetAdminsWithPaginationWithSearchWithFilterQuery : IRequest<PaginatedList<AdminDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsAdminMustBeVerified { get; init; }
    public Guid? WithoutAdminId { get; init; }
    public bool? IsSuperAdmin { get; init; }
    public ActivationStatus? ActivationStatus { get; init; }
}

public class GetAdminsWithPaginationWithSearchWithFilterQueryHandler : IRequestHandler<GetAdminsWithPaginationWithSearchWithFilterQuery, PaginatedList<AdminDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAdminsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<AdminDTO>> Handle(GetAdminsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Admins
            .AsNoTracking()
            .Filter(
                withoutAdminId: request.WithoutAdminId,
                isVerified: request.IsAdminMustBeVerified,
                isSuperAdmin: request.IsSuperAdmin,
                activationStatus: request.ActivationStatus
             )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Email)
            .Select(x => new AdminDTO
            {
                Id = x.Id,
                Email = x.Email,
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                IsSuperAdmin = x.IsSuperAdmin,
                ActivationStatus = x.ActivationStatus,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}