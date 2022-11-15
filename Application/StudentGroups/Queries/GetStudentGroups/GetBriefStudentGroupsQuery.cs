using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries.GetStudentGroups;

public record GetBriefStudentGroupsQuery
    : IRequest<IEnumerable<BriefStudentGroupDTO>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetBriefStudentGroupsQueryHandler
    : IRequestHandler<GetBriefStudentGroupsQuery, IEnumerable<BriefStudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefStudentGroupsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BriefStudentGroupDTO>> Handle(
        GetBriefStudentGroupsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .MapToBriefStudentGroupDTO()
            .ToListAsync();
    }
}