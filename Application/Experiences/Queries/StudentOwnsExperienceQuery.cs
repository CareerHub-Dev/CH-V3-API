using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries;

public record StudentOwnsExperienceQuery : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public bool IsStudentVerified { get; init; }
    public Guid ExperienceId { get; init; }
}

public class StudentOwnsExperienceQueryHandler : IRequestHandler<StudentOwnsExperienceQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public StudentOwnsExperienceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(StudentOwnsExperienceQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students.Filter(isVerified: request.IsStudentVerified).AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Experiences
                .AnyAsync(x => x.StudentId == request.StudentId && x.Id == request.ExperienceId);
    }
}