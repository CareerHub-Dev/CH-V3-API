using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Commands.DeleteStudentGroup;

public record DeleteStudentGroupCommand(Guid StudentGroupId)
    : IRequest;

public class DeleteStudentGroupCommandHandler
    : IRequestHandler<DeleteStudentGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentGroupCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteStudentGroupCommand request, CancellationToken cancellationToken)
    {
        var studentGroup = await _context.StudentGroups
            .FirstOrDefaultAsync(x => x.Id == request.StudentGroupId);

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        _context.StudentGroups.Remove(studentGroup);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
