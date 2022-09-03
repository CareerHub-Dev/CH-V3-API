using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Tag;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries;

public record GetTagBriefsWithSearchQuery : IRequest<IEnumerable<TagBriefDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetTagBriefsWithSearchQueryHandler : IRequestHandler<GetTagBriefsWithSearchQuery, IEnumerable<TagBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetTagBriefsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TagBriefDTO>> Handle(GetTagBriefsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderByDescending(x => x.Name)
            .Select(x => new TagBriefDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}