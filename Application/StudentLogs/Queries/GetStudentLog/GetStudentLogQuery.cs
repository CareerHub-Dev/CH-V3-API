using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.StudentLogs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetStudentLog;

public record GetStudentLogQuery(Guid StudentLogId) : IRequest<StudentLogDTO>;

public class GetStudentLogQueryHandler : IRequestHandler<GetStudentLogQuery, StudentLogDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentLogDTO> Handle(GetStudentLogQuery request, CancellationToken cancellationToken)
    {
        var studentGroup = await _context.StudentLogs
            .AsNoTracking()
            .Where(x => x.Id == request.StudentLogId)
            .Select(x => new StudentLogDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Created = x.Created,
                LastModified = x.LastModified,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name }
            })
            .FirstOrDefaultAsync();

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentLog), request.StudentLogId);
        }

        return studentGroup;
    }
}