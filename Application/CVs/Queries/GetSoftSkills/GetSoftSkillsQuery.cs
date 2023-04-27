using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetSoftSkills;

public class GetSoftSkillsQuery
    : IRequest<List<string>>
{

}

public class GetSoftSkillsQueryHandler
    : IRequestHandler<GetSoftSkillsQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetSoftSkillsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(
        GetSoftSkillsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.CVs.SelectMany(x => x.SoftSkills).DistinctBy(x => x.Trim().ToLower()).ToListAsync();
    }
}