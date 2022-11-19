using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetBriefTag;

public record GetBriefTagQuery(Guid TagId)
    : IRequest<BriefTagDTO>;

public class GetBriefTagQueryHandler
    : IRequestHandler<GetBriefTagQuery, BriefTagDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefTagQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefTagDTO> Handle(
        GetBriefTagQuery request,
        CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .MapToBriefTagDTO()
            .FirstOrDefaultAsync(x => x.Id == request.TagId);

        if (tag == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        return tag;
    }
}
