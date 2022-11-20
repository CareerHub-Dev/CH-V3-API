using Application.Common.DTO.Bans;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bans.Queries.GetBan;

public record GetBanQuery(Guid BanId)
    : IRequest<BanDTO>;

public class GetBanQueryHandler
    : IRequestHandler<GetBanQuery, BanDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBanQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BanDTO> Handle(
        GetBanQuery request,
        CancellationToken cancellationToken)
    {
        var ban = await _context.Bans
            .MapToBanDTO()
            .FirstOrDefaultAsync(x => x.Id == request.BanId);

        if (ban == null)
        {
            throw new NotFoundException(nameof(Ban), request.BanId);
        }

        return ban;
    }
}