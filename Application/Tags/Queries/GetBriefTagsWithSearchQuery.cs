using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Tag;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries;

public record GetBriefTagsWithSearchQuery : IRequest<IList<BriefTagDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetBriefTagsWithSearchQueryHandler : IRequestHandler<GetBriefTagsWithSearchQuery, IList<BriefTagDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefTagsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<BriefTagDTO>> Handle(GetBriefTagsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new BriefTagDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}