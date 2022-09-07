using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.StudentOwnerSubscribeToStudentTargetWithConfiguration;

public record StudentOwnerSubscribeToStudentTargetWithConfigurationCommand : IRequest
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerVerified { get; init; }
    public Guid StudentTargetId { get; init; }
    public bool? IsStudentTargetVerified { get; init; }
}

public class StudentOwnerSubscribeToStudentTargetWithConfigurationCommandHandler : IRequestHandler<StudentOwnerSubscribeToStudentTargetWithConfigurationCommand>
{
    private readonly IApplicationDbContext _context;

    public StudentOwnerSubscribeToStudentTargetWithConfigurationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StudentOwnerSubscribeToStudentTargetWithConfigurationCommand request, CancellationToken cancellationToken)
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

        var isStudentSubscriptionExists = await _context.StudentSubscriptions.AnyAsync(x =>
            x.SubscriptionOwnerId == request.StudentOwnerId &&
            x.SubscriptionTargetId == request.StudentTargetId
        );

        if(isStudentSubscriptionExists)
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