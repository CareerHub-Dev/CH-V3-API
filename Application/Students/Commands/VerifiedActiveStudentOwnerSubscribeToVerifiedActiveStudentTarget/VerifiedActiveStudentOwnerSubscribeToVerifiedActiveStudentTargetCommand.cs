using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTarget;

public record VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommand : IRequest
{
    public Guid StudentOwnerId { get; init; }
    public Guid StudentTargetId { get; init; }
}

public class VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommandHandler
    : IRequestHandler<VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommand request, 
        CancellationToken cancellationToken)
    {
        if (request.StudentOwnerId == request.StudentTargetId)
        {
            throw new ArgumentException("StudentOwnerId and StudentTargetId are same.");
        }

        if (!await _context.Students
            .Filter(
                isVerified: true, 
                activationStatus: ActivationStatus.Active
            )
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        if (!await _context.Students
            .Filter(
                isVerified: true,
                activationStatus: ActivationStatus.Active
            )
            .AnyAsync(x => x.Id == request.StudentTargetId))
        {
            throw new NotFoundException(nameof(Student), request.StudentTargetId);
        }

        var isStudentSubscriptionExists = await _context.StudentSubscriptions.AnyAsync(x =>
            x.SubscriptionOwnerId == request.StudentOwnerId &&
            x.SubscriptionTargetId == request.StudentTargetId
        );

        if (isStudentSubscriptionExists)
        {
            return Unit.Value;
        }

        _context.StudentSubscriptions.Add(new StudentSubscription
        {
            SubscriptionOwnerId = request.StudentOwnerId,
            SubscriptionTargetId = request.StudentTargetId
        });

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}