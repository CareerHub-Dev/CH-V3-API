using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTags;

public record GetBriefTagsWithSearchQuery : IRequest<IEnumerable<BriefTagDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetBriefTagsWithSearchQueryHandler : IRequestHandler<GetBriefTagsWithSearchQuery, IEnumerable<BriefTagDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefTagsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BriefTagDTO>> Handle(GetBriefTagsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .MapToBriefTagDTO()
            .ToListAsync();
    }
}