using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record IsStudentOwnerSubscribedToStudentTargetQuery : IRequest<bool>
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerVerified { get; init; }
    public Guid StudentTargetId { get; init; }
    public bool? IsStudentTargetVerified { get; init; }
}

public class IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQueryHandler : IRequestHandler<IsStudentOwnerSubscribedToStudentTargetQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsStudentOwnerSubscribedToStudentTargetQuery request, CancellationToken cancellationToken)
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

        return await _context.StudentSubscriptions.AnyAsync(x => 
            x.SubscriptionOwnerId == request.StudentOwnerId &&
            x.SubscriptionTargetId == request.StudentTargetId
        );
    }
}