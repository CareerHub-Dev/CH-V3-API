using API.Authorize;
using API.DTO.Requests.Notifications;
using Application.Common.DTO.Notifications;
using Application.Common.Enums;
using Application.Notifications.Commands.SetViewedNotificationOfStudentCommand;
using Application.Notifications.Queries.GetAmountUnviewedNotificationsOfStudent;
using Application.Notifications.Queries.GetNotificationsOfStudentWithPaginig;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class NotificationsController : ApiControllerBase
{
    [HttpPost("self/set-viewed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetViewedNotificationOfSelfStudent(SetViewedOwnNotificationRequest request)
    {
        await Sender.Send(new SetViewedNotificationOfStudentCommand
        {
            NotificationId = request.NotificationId,
            StudentId = AccountInfo!.Id,
        });

        return NoContent();
    }

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NotificationDTO>))]
    public async Task<IActionResult> GetNotificationsOfSelfStudent(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await Sender.Send(new GetNotificationsOfStudentWithPaginigQuery
        {
            StudentId = AccountInfo!.Id,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = "Created DESC"
        }));
    }

    [HttpGet("self/amount-unviewed")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountUnviewedNotificationsOfSelfStudent()
    {
        return Ok(await Sender.Send(new GetAmountUnviewedNotificationsOfStudentQuery
        {
            StudentId = AccountInfo!.Id,
        }));
    }
}
