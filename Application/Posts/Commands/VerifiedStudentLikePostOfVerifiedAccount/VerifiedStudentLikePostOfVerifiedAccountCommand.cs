using Application.Common.DTO.Notifications;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Posts.Commands.VerifiedStudentLikePostOfVerifiedAccount;

public record VerifiedStudentLikePostOfVerifiedAccountCommand
    : IRequest
{
    public Guid StudentId { get; init; }
    public Guid PostId { get; init; }
}

public class VerifiedStudentLikePostOfVerifiedAccountCommandHandler
    : IRequestHandler<VerifiedStudentLikePostOfVerifiedAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IBaseUrlService _baseUrlService;

    public VerifiedStudentLikePostOfVerifiedAccountCommandHandler(
        IApplicationDbContext context,
        INotificationService notificationService,
        IBaseUrlService baseUrlService)
    {
        _context = context;
        _notificationService = notificationService;
        _baseUrlService = baseUrlService;
    }

    public async Task<Unit> Handle(
        VerifiedStudentLikePostOfVerifiedAccountCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var post = await _context.Posts
           .Include(x => x.StudentsLiked)
           .Include(x => x.Account)
           .Filter(isAccountVerified: true)
           .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        if (post.StudentsLiked.Any(x => x.Id == request.StudentId))
        {
            return Unit.Value;
        }

        post.StudentsLiked.Add(student);

        var postAuthorIsStudent = await _context.Students
            .AsNoTracking()
            .Filter(isVerified: true)
            .AnyAsync(x => x.Id == post.AccountId);

        if (!postAuthorIsStudent)
        {
            await _context.SaveChangesAsync();
            return Unit.Value;
        }

        var notification = new Notification
        {
            ReferenceId = post.Id,
            EnMessage = $"Your post \"{post.Text}\" was liked",
            UkMessage = $"Твій пост \"{post.Text}\" оцінили!",
            Image = post.Images.FirstOrDefault(),
            IsViewed = false,
            Created = DateTime.UtcNow,
            Type = Domain.Enums.NotificationType.Post,
            StudentId = post.AccountId,
        };

        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        if(post.Account!.PlayerId.HasValue)
        {
            await _notificationService.SendNotificationAsync(
                new List<Guid> { post.Account.PlayerId.Value },
                $"{_baseUrlService.GetBaseUrl()}main/posts/{post.Id}",
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
                    Created = notification.Created,
                    Type = notification.Type,
                });
        }

        return Unit.Value;
    }
}