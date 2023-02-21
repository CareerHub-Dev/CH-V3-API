using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Notifications.Queries.GetAmountUnviewedNotificationsOfStudent;

public class GetAmountUnviewedNotificationsOfStudentQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
}

public class GetAmountUnviewedNotificationsOfStudentQueryHandler
    : IRequestHandler<GetAmountUnviewedNotificationsOfStudentQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountUnviewedNotificationsOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountUnviewedNotificationsOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(x => x.StudentId == request.StudentId)
            .Where(x => !x.IsViewed)
            .CountAsync();
    }
}