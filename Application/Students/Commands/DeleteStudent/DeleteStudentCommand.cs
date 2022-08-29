using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid StudentId) : IRequest;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        _context.Students.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}