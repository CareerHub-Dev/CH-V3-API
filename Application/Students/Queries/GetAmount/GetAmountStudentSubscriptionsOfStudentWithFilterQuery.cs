using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmount;

public record GetAmountStudentSubscriptionsOfStudentWithFilterQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }

    public bool? IsStudentTargetOfSubscriptionMustBeVerified { get; init; }
    public ActivationStatus? StudentTargetOfSubscriptionMustHaveActivationStatus { get; init; }
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
            .Filter(
                isVerified: request.IsStudentMustBeVerified,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.StudentSubscriptions
            .Where(x => x.SubscriptionOwnerId == request.StudentId)
            .Select(x => x.SubscriptionTarget)!
            .Filter(
                isVerified: request.IsStudentTargetOfSubscriptionMustBeVerified, 
                activationStatus: request.StudentTargetOfSubscriptionMustHaveActivationStatus
            )
            .CountAsync();
    }
}