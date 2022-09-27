﻿using Application.Common.DTO.JobPositions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetJobPosition;

public record GetBriefJobPositionQuery(Guid JobPositionId) : IRequest<BriefJobPositionDTO>;

public class GetBriefJobPositionQueryHandler : IRequestHandler<GetBriefJobPositionQuery, BriefJobPositionDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefJobPositionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefJobPositionDTO> Handle(GetBriefJobPositionQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.JobPositions
            .AsNoTracking()
            .Where(x => x.Id == request.JobPositionId)
            .Select(x => new BriefJobPositionDTO
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        return tag;
    }
}