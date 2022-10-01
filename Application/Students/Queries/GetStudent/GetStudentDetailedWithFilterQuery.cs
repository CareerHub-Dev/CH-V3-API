using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudent;

public record GetStudentDetailedWithFilterQuery : IRequest<StudentDetailedDTO>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }
}

public class GetStudentDetailedWithFilterQueryHandler
    : IRequestHandler<GetStudentDetailedWithFilterQuery, StudentDetailedDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentDetailedWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentDetailedDTO> Handle(GetStudentDetailedWithFilterQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(x => x.Id == request.StudentId)
            .Filter(
                isVerified: request.IsStudentMustBeVerified,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .Select(x => new StudentDetailedDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhotoId = x.PhotoId,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
            })
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student;
    }
}