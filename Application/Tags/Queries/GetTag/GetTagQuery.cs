using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTag;

public record GetTagQuery(Guid TagId) : IRequest<TagDTO>;

public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagDTO>
{
    private readonly IApplicationDbContext _context;

    public GetTagQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TagDTO> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .AsNoTracking()
            .Where(x => x.Id == request.TagId)
            .MapToTagDTO()
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        return tag;
    }
}