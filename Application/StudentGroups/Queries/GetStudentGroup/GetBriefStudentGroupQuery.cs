using Application.Common.DTO.StudentGroups;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetStudentGroup;

public record GetBriefStudentGroupQuery(Guid StudentGroupId) : IRequest<BriefStudentGroupDTO>;

public class GetBriefStudentGroupQueryHandler : IRequestHandler<GetBriefStudentGroupQuery, BriefStudentGroupDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefStudentGroupQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefStudentGroupDTO> Handle(GetBriefStudentGroupQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.StudentGroups
            .AsNoTracking()
            .Where(x => x.Id == request.StudentGroupId)
            .Select(x => new BriefStudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        return tag;
    }
}
