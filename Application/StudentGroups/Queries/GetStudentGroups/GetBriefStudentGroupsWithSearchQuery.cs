using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries.GetStudentGroups;

public record GetBriefStudentGroupsWithSearchQuery : IRequest<IList<BriefStudentGroupDTO>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetBriefStudentGroupsWithSearchQueryHandler : IRequestHandler<GetBriefStudentGroupsWithSearchQuery, IList<BriefStudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefStudentGroupsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<BriefStudentGroupDTO>> Handle(GetBriefStudentGroupsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .AsNoTracking()
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .Select(x => new BriefStudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}