using Application.Common.DTO.Notifications;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Services.Notifications;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.VerifiedStudentOwnerSubscribeToVerifiedStudentTarget;

public record VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommand
    : IRequest
{
    public Guid StudentOwnerId { get; init; }
    public Guid StudentTargetId { get; init; }
}

public class VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommandHandler
    : IRequestHandler<VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IBaseUrlService _baseUrlService;

    public VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommandHandler(
        IApplicationDbContext context,
        INotificationService notificationService,
        IBaseUrlService baseUrlService)
    {
        _context = context;
        _notificationService = notificationService;
        _baseUrlService = baseUrlService;
    }

    public async Task<Unit> Handle(
        VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommand request,
        CancellationToken cancellationToken)
    {
        if (request.StudentOwnerId == request.StudentTargetId)
        {
            throw new ArgumentException("StudentOwnerId and StudentTargetId are same.");
        }

        var studentOwner = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentOwnerId);

        if (studentOwner == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        var studentTarget = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentTargetId);

        if (studentTarget == null)
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

        var notification = new Notification
        {
            ReferenceId = studentOwner.Id,
            EnMessage = $"{studentOwner.LastName} {studentOwner.FirstName} start following you!",
            UkMessage = $"{studentOwner.LastName} {studentOwner.FirstName} підписався на тебе!",
            Image = studentOwner.Photo,
            IsViewed = false,
            Created = DateTime.UtcNow,
            Type = Domain.Enums.NotificationType.Student,
            StudentId = request.StudentTargetId,
        };

        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        if (studentTarget.PlayerId.HasValue)
        {
            await _notificationService.SendNotificationAsync(
                new List<Guid> { studentTarget.PlayerId.Value },
                $"{_baseUrlService.GetBaseUrl()}main/students/{request.StudentOwnerId}",
                notification.EnMessage,
                notification.UkMessage,
                data: new NotificationDTO
                {
                    Id = notification.Id,
                    ReferenceId = notification.ReferenceId,
                    EnMessage = notification.EnMessage,
                    UkMessage = notification.UkMessage,
                    Image = notification.Image,
                    IsViewed = notification.IsViewed,
                    Created  = notification.Created,
                    Type = notification.Type,
                });
        }

        return Unit.Value;
    }
}