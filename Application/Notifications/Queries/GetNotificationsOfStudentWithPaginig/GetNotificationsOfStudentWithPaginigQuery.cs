using Application.Common.DTO.Notifications;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;

namespace Application.Notifications.Queries.GetNotificationsOfStudentWithPaginig;

public class GetNotificationsOfStudentWithPaginigQuery
    : IRequest<PaginatedList<NotificationDTO>>
{
    public Guid StudentId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetNotificationsOfStudentWithPaginigQueryHandler
    : IRequestHandler<GetNotificationsOfStudentWithPaginigQuery, PaginatedList<NotificationDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetNotificationsOfStudentWithPaginigQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<NotificationDTO>> Handle(
        GetNotificationsOfStudentWithPaginigQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(x => x.StudentId == request.StudentId)
            .MapToNotificationDTO()
            .OrderByDescending(x => x.IsViewed)
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}