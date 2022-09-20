using Application.Common.DTO.JobPositions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetJobPosition;

public record GetJobPositionQuery(Guid JobPositionId) : IRequest<JobPositionDTO>;

public class GetJobPositionQueryHandler : IRequestHandler<GetJobPositionQuery, JobPositionDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobPositionDTO> Handle(GetJobPositionQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.JobPositions
            .AsNoTracking()
            .Where(x => x.Id == request.JobPositionId)
            .Select(x => new JobPositionDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                LastModified = x.LastModified,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
            })
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        return tag;
    }
}