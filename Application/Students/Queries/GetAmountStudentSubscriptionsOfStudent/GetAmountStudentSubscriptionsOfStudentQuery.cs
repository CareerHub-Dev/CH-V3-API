using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmountStudentSubscriptionsOfStudent;

public record GetAmountStudentSubscriptionsOfStudentQuery
    : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public bool? IsStudentTargetOfSubscriptionMustBeVerified { get; init; }
}

public class GetAmountStudentSubscriptionsOfStudentQueryHandler
    : IRequestHandler<GetAmountStudentSubscriptionsOfStudentQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountStudentSubscriptionsOfStudentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountStudentSubscriptionsOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentMustBeVerified
            )
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.StudentSubscriptions
            .Where(x => x.SubscriptionOwnerId == request.StudentId)
            .Select(x => x.SubscriptionTarget)!
            .Filter(isVerified: request.IsStudentTargetOfSubscriptionMustBeVerified)
            .CountAsync();
    }
}