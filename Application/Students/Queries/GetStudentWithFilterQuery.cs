using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.StudentGroup;
using Application.Students.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record GetStudentWithFilterQuery : IRequest<StudentDTO>
{
    public Guid StudentId { get; init; }
    public bool? IsVerified { get; init; }
}

public class GetStudentWithFilterQueryHandler
    : IRequestHandler<GetStudentWithFilterQuery, StudentDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentDTO> Handle(GetStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(x => x.Id == request.StudentId)
            .Filter(isVerified: request.IsVerified)
            .Select(x => new StudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhotoId = x.PhotoId,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new StudentGroupBriefDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset
            })
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student;
    }
}