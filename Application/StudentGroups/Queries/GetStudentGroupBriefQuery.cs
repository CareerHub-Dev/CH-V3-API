using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.StudentGroup;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupBriefQuery(Guid StudentGroupId) : IRequest<StudentGroupBriefDTO>;

public class GetStudentGroupBriefQueryHandler : IRequestHandler<GetStudentGroupBriefQuery, StudentGroupBriefDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupBriefQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentGroupBriefDTO> Handle(GetStudentGroupBriefQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentGroups
            .AsNoTracking()
            .Where(x => x.Id == request.StudentGroupId)
            .Select(x => new StudentGroupBriefDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        return entity;
    }
}