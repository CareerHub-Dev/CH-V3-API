using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmount;

public record GetAmountStudentSubscribersOfStudentWithFilterQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }

    public bool? IsStudentOwnerOfSubscriptionMustBeVerified { get; init; }
    public ActivationStatus? StudentOwnerOfSubscriptionMustHaveActivationStatus { get; init; }
}

public class GetAmountStudentSubscribersOfStudentWithFilterQueryHandler
    : IRequestHandler<GetAmountStudentSubscribersOfStudentWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountStudentSubscribersOfStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountStudentSubscribersOfStudentWithFilterQuery request, CancellationToken cancellationToken)
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
            .Where(x => x.SubscriptionTargetId == request.StudentId)
            .Select(x => x.SubscriptionOwner)!
            .Filter(
                isVerified: request.IsStudentOwnerOfSubscriptionMustBeVerified, 
                activationStatus: request.StudentOwnerOfSubscriptionMustHaveActivationStatus
            )
            .CountAsync();
    }
}