using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetHardSkills;

public class GetHardSkillsQuery
    : IRequest<List<string>>
{

}

public class GetHardSkillsQueryHandler
    : IRequestHandler<GetHardSkillsQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetHardSkillsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(
        GetHardSkillsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.CVs.SelectMany(x => x.HardSkills).DistinctBy(x => x.Trim().ToLower()).ToListAsync();
    }
}