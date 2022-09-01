using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand : IRequest
{
    public Guid StudentId { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public DateTime? BirthDate { get; init; }
    public Guid StudentGroupId { get; init; }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if (!await _context.StudentGroups.AnyAsync(x => x.Id == request.StudentGroupId))
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.Phone = request.Phone;
        student.StudentGroupId = request.StudentGroupId;
        student.BirthDate = request.BirthDate;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}