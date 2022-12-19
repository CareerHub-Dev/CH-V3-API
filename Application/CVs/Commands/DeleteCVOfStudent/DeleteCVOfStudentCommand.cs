using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.DeleteCVOfStudent;

public record DeleteCVOfStudentCommand(Guid CVId, Guid StudentId)
    : IRequest;

public class DeleteCVOfStudentCommandHandler
    : IRequestHandler<DeleteCVOfStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCVOfStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteCVOfStudentCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        _context.CVs.Remove(cv);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}