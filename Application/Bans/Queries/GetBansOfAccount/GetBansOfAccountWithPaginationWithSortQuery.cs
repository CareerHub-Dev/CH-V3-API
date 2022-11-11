using Application.Bans.Queries.GetBan;
using Application.Common.DTO.Bans;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bans.Queries.GetBansOfAccount;

public record GetBansOfAccountWithPaginationWithSortQuery : IRequest<PaginatedList<BanDTO>>
{
    public Guid AccountId { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBansOfAccountWithPaginationWithSortQueryHandler : IRequestHandler<GetBansOfAccountWithPaginationWithSortQuery, PaginatedList<BanDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBansOfAccountWithPaginationWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BanDTO>> Handle(GetBansOfAccountWithPaginationWithSortQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var bans = await _context.Bans
            .AsNoTracking()
            .Where(x => x.AccountId == request.AccountId)
            .MapToBanDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);

        return bans;
    }
}