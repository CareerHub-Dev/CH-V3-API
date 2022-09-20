using Application.Common.DTO.Tags;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTag;

public record GetBriefTagQuery(Guid TagId) : IRequest<BriefTagDTO>;

public class GetBriefTagQueryHandler : IRequestHandler<GetBriefTagQuery, BriefTagDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefTagQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefTagDTO> Handle(GetBriefTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .AsNoTracking()
            .Where(x => x.Id == request.TagId)
            .Select(x => new BriefTagDTO
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        return tag;
    }
}
