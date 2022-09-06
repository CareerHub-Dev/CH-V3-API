using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record GetAmountStudentSubscriptionsOfStudentWithFilterQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsVerified { get; init; }

    public bool? IsStudentTargetOfSubscriptionVerified { get; init; }
}

public class GetAmountStudentSubscriptionsOfStudentWithFilterQueryHandler
    : IRequestHandler<GetAmountStudentSubscriptionsOfStudentWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountStudentSubscriptionsOfStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountStudentSubscriptionsOfStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.StudentSubscriptions
            .Where(x => x.SubscriptionOwnerId == request.StudentId)
            .Select(x => x.SubscriptionTarget)!
            .Filter(isVerified: request.IsStudentTargetOfSubscriptionVerified)
            .CountAsync();
    }
}