using Application.Accounts.Commands.RegisterStudent;
using Application.Accounts.Commands.ResetPassword;
using Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;
using Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Accounts.Commands.VerifyStudent;
using Application.Accounts.Queries.Authenticate;
using Application.Accounts.Queries.RefreshToken;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Common.Models.Account;

namespace WebUI.Areas;

[Route("api/[controller]")]
public class AccountsController : ApiControllerBase
{
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateResult))]
    public async Task<IActionResult> Authenticate(AuthenticateQuery query)
    {
        var response = await Mediator.Send(query);

        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResult))]
    public async Task<IActionResult> RefreshToken(RefreshTokenView view)
    {
        if (string.IsNullOrWhiteSpace(view.Token))
        {
            view.Token = Request.Cookies["refreshToken"] ?? "";
        }

        var response = await Mediator.Send(new RefreshTokenQuery
        {
            Token = view.Token
        });

        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("verify-company-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyCompanyWithContinuedRegistration([FromForm] VerifyCompanyWithContinuedRegistrationCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpPost("verify-admin-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyAdminWithContinuedRegistration(VerifyAdminWithContinuedRegistrationCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpPost("verify-student-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyStudent(VerifyStudentCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpPost("register/student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterStudent(RegisterStudentCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword(SendPasswordResetEmailCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    // helper methods
    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true,
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
