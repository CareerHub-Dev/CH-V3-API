using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.StudentOwnerUnsubscribeFromStudentTarget;

public record StudentOwnerUnsubscribeFromStudentTargetCommand : IRequest
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerVerified { get; init; }
    public Guid StudentTargetId { get; init; }
    public bool? IsStudentTargetVerified { get; init; }
}

public class StudentOwnerUnsubscribeFromStudentTargetCommandHandler : IRequestHandler<StudentOwnerUnsubscribeFromStudentTargetCommand>
{
    private readonly IApplicationDbContext _context;

    public StudentOwnerUnsubscribeFromStudentTargetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StudentOwnerUnsubscribeFromStudentTargetCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentOwnerId == request.StudentTargetId)
        {
            throw new ArgumentException("StudentOwnerId and StudentTargetId are same.");
        }

        if (!await _context.Students.Filter(isVerified: request.IsStudentOwnerVerified).AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException("StudentOwner", request.StudentOwnerId);
        }

        if (!await _context.Students.Filter(isVerified: request.IsStudentTargetVerified).AnyAsync(x => x.Id == request.StudentTargetId))
        {
            throw new NotFoundException("StudentTarget", request.StudentTargetId);
        }

        var StudentSubscription = await _context.StudentSubscriptions.AsNoTracking().FirstOrDefaultAsync(x =>
            x.SubscriptionOwnerId == request.StudentOwnerId &&
            x.SubscriptionTargetId == request.StudentTargetId
        );

        if (StudentSubscription == null)
        {
            return Unit.Value;
        }

        _context.StudentSubscriptions.Remove(StudentSubscription);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}