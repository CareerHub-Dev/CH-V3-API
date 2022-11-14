using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid StudentId)
    : IRequest;

public class DeleteStudentCommandHandler
    : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteStudentCommand request, 
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Where(x => x.Id == request.StudentId)
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        _context.Students.Remove(student);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}