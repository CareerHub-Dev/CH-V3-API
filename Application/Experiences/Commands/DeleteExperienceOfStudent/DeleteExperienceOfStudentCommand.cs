using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Commands.DeleteExperienceOfStudent;

public record DeleteExperienceOfStudentCommand(Guid ExperienceId, Guid StudentId) : IRequest;

public class DeleteExperienceOfStudentCommandHandler : IRequestHandler<DeleteExperienceOfStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceOfStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteExperienceOfStudentCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var experience = await _context.Experiences
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ExperienceId && x.StudentId == request.StudentId);

        if (experience == null)
        {
            throw new NotFoundException(nameof(Experience), request.ExperienceId);
        }

        _context.Experiences.Remove(experience);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}