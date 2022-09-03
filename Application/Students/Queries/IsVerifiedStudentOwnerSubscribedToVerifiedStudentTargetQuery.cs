﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQuery : IRequest<bool>
{
    public Guid StudentOwnerId { get; init; }
    public Guid StudentTargetId { get; init; }
}

public class IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQueryHandler : IRequestHandler<IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQuery request, CancellationToken cancellationToken)
    {
        if (request.StudentOwnerId == request.StudentTargetId)
        {
            throw new ArgumentException("StudentOwnerId and StudentTargetId are same.");
        }

        if (!await _context.Students.Filter(IsVerified: true).AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException("StudentOwner", request.StudentOwnerId);
        }

        if (!await _context.Students.Filter(IsVerified: true).AnyAsync(x => x.Id == request.StudentTargetId))
        {
            throw new NotFoundException("StudentTarget", request.StudentTargetId);
        }

        return await _context.StudentSubscriptions.AnyAsync(x => 
            x.SubscriptionOwnerId == request.StudentOwnerId &&
            x.SubscriptionTargetId == request.StudentTargetId
        );
    }
}