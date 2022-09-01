using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Commands.UpdateStudentGroup;

public class UpdateStudentGroupCommand : IRequest
{
    public Guid StudentGroupId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class UpdateStudentGroupCommandHandler : IRequestHandler<UpdateStudentGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateStudentGroupCommand request, CancellationToken cancellationToken)
    {
        var studentGroup = await _context.StudentGroups
            .FirstOrDefaultAsync(x => x.Id == request.StudentGroupId);

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        studentGroup.Name = request.Name;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
