using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.StudentGroups.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupQuery(Guid StudentGroupId) : IRequest<StudentGroupDTO>;

public class GetStudentGroupQueryHandler : IRequestHandler<GetStudentGroupQuery, StudentGroupDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentGroupDTO> Handle(GetStudentGroupQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentGroups
            .AsNoTracking()
            .Where(x => x.Id == request.StudentGroupId)
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        return entity;
    }
}
