using Application.Common.DTO.Notifications;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;

namespace Application.Notifications.Queries.GetUnviewedNotificationsOfStudentWithPaginig;

public class GetUnviewedNotificationsOfStudentWithPaginigQuery
    : IRequest<PaginatedList<NotificationDTO>>
{
    public Guid StudentId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetUnviewedNotificationsOfStudentWithPaginigQueryHandler
    : IRequestHandler<GetUnviewedNotificationsOfStudentWithPaginigQuery, PaginatedList<NotificationDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetUnviewedNotificationsOfStudentWithPaginigQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<NotificationDTO>> Handle(
        GetUnviewedNotificationsOfStudentWithPaginigQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Filter(isViewed: false)
            .Where(x => x.StudentId == request.StudentId)
            .MapToNotificationDTO()
            .OrderByDescending(x => x.IsViewed)
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}