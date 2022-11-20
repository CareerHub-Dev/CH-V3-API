using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetBriefJobPosition;

public record GetBriefJobPositionQuery(Guid JobPositionId)
    : IRequest<BriefJobPositionDTO>;

public class GetBriefJobPositionQueryHandler
    : IRequestHandler<GetBriefJobPositionQuery, BriefJobPositionDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefJobPositionQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefJobPositionDTO> Handle(
        GetBriefJobPositionQuery request,
        CancellationToken cancellationToken)
    {
        var tag = await _context.JobPositions
            .MapToBriefJobPositionDTO()
            .FirstOrDefaultAsync(x => x.Id == request.JobPositionId);

        if (tag == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        return tag;
    }
}
