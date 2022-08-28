using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Student;

namespace WebUI.Controllers;

public class StudentsController : ApiControllerBase
{
    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-verification-email")]
    [Authorize("Admin")]
    public async Task<IActionResult> SendInviteCompanyEmail(SendVerifyStudentEmailRequest sendVerifyStudentEmail)
    {
        await Mediator.Send(new SendVerifyStudentEmailCommand(sendVerifyStudentEmail.StudentId));
        return Ok();
    }
}
