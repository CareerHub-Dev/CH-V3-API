using Application.Accounts.Commands.SetPayerId;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Notifications.Commands.SetViewedNotificationOfStudentCommand;

public class SetViewedNotificationOfStudentCommand : IRequest
{
    public Guid NotificationId { get; init; }
    public Guid StudentId { get; init; }
}

public class SetViewedNotificationOfStudentCommandHandler
    : IRequestHandler<SetViewedNotificationOfStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public SetViewedNotificationOfStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        SetViewedNotificationOfStudentCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == request.NotificationId && x.StudentId == request.StudentId);

        if (notification == null)
        {
            throw new NotFoundException(nameof(Account), request.NotificationId);
        }

        notification.IsViewed = true;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}