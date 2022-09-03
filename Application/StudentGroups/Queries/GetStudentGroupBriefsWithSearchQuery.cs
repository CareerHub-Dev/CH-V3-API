using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.StudentGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupBriefsWithSearchQuery : IRequest<IEnumerable<StudentGroupBriefDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetStudentGroupBriefsWithSearchQueryHandler : IRequestHandler<GetStudentGroupBriefsWithSearchQuery, IEnumerable<StudentGroupBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupBriefsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentGroupBriefDTO>> Handle(GetStudentGroupBriefsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentGroups
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderByDescending(x => x.Name)
            .Select(x => new StudentGroupBriefDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}