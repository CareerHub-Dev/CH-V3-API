using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.StudentOwnerUnsubscribeFromStudentTarget;

public record VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommand : IRequest
{
    public Guid StudentOwnerId { get; init; }
    public Guid StudentTargetId { get; init; }
}

public class VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommandHandler : IRequestHandler<VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentOwnerId == request.StudentTargetId)
        {
            throw new ArgumentException("StudentOwnerId and StudentTargetId are same.");
        }

        if (!await _context.Students.Filter(isVerified: true).AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        if (!await _context.Students.Filter(isVerified: true).AnyAsync(x => x.Id == request.StudentTargetId))
        {
            throw new NotFoundException(nameof(Student), request.StudentTargetId);
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