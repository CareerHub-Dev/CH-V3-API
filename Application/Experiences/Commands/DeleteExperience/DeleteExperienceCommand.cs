using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Commands.DeleteExperience;

public record DeleteExperienceCommand(Guid ExperienceId)
    : IRequest;

public class DeleteExperienceCommandHandler
    : IRequestHandler<DeleteExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteExperienceCommand request, 
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .FirstOrDefaultAsync(x => x.Id == request.ExperienceId);

        if (experience == null)
        {
            throw new NotFoundException(nameof(Experience), request.ExperienceId);
        }

        _context.Experiences.Remove(experience);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}