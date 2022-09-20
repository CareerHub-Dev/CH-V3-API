using Application.Common.DTO.StudentGroups;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetStudentGroup;

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
        var studentGroup = await _context.StudentGroups
            .AsNoTracking()
            .Where(x => x.Id == request.StudentGroupId)
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                LastModified = x.LastModified,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
            })
            .FirstOrDefaultAsync();

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        return studentGroup;
    }
}