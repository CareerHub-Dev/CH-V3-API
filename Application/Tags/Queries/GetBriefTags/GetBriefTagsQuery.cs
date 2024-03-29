﻿using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetBriefTags;

public record GetBriefTagsQuery
    : IRequest<IEnumerable<BriefTagDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetBriefTagsQueryHandler
    : IRequestHandler<GetBriefTagsQuery, IEnumerable<BriefTagDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefTagsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BriefTagDTO>> Handle(
        GetBriefTagsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .MapToBriefTagDTO()
            .ToListAsync();
    }
}